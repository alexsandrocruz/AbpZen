using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;

namespace Volo.Payment.PayPal;

public class PayPalPaymentGateway : IPaymentGateway, ITransientDependency
{
    protected PayPalHttpClient PayPalHttpClient { get; }
    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    public PayPalPaymentGateway(PayPalHttpClient payPalHttpClient,
        IPurchaseParameterListGenerator purchaseParameterListGenerator,
        IPaymentRequestRepository paymentRequestRepository)
    {
        PayPalHttpClient = payPalHttpClient;
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        PaymentRequestRepository = paymentRequestRepository;
    }

    public virtual async Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var purchaseParameters = PurchaseParameterListGenerator.GetExtraParameterConfiguration(paymentRequest);

        // TODO: Remove currency fallback in next major version.
        // Backward support
        var currency = paymentRequest.Currency.IsNullOrEmpty() ? purchaseParameters.CurrencyCode : paymentRequest.Currency;

        var order = new OrderRequest
        {
            CheckoutPaymentIntent = "CAPTURE",
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = input.ReturnUrl,
                CancelUrl = input.CancelUrl,
                Locale = purchaseParameters.Locale
            },
            PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = currency,
                                    Value = paymentRequest.Products.Sum(product => product.TotalPrice).ToString(".00", CultureInfo.InvariantCulture)
                                }
                            },
                            CurrencyCode = currency,
                            Value = paymentRequest.Products.Sum(product => product.TotalPrice).ToString(".00", CultureInfo.InvariantCulture),
                        },
                        Items = paymentRequest.Products.Select(product => new Item
                        {
                            Quantity = product.Count.ToString(),
                            Name = product.Name,
                            UnitAmount = new Money
                            {
                                CurrencyCode = currency,
                                Value = product.UnitPrice.ToString(".00", CultureInfo.InvariantCulture)
                            }
                        }).ToList(),
                        ReferenceId = paymentRequest.Id.ToString()
                    }
                }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);

        var result = (await PayPalHttpClient.Execute(request)).Result<Order>();

        return new PaymentRequestStartResult
        {
            CheckoutLink = result.Links.First(x => x.Rel == "approve").Href
        };
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var request = new OrdersCaptureRequest(parameters["Token"]);
        request.RequestBody(new OrderActionRequest());

        var order = (await PayPalHttpClient.Execute(request)).Result<Order>();

        var paymentRequestId = Guid.Parse(order.PurchaseUnits.First().ReferenceId);

        return await UpdatePaymentRequestStatusAsync(paymentRequestId, order);
    }

    public virtual async Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        // TODO: Find better way to parse.
        var jObject = JObject.Parse(payload);

        var order = jObject["resource"].ToObject<Order>();

        var request = new OrdersGetRequest(order.Id);

        // Ensure order object comes from PayPal
        order = (await PayPalHttpClient.Execute(request)).Result<Order>();

        var paymentRequestId = Guid.Parse(order.PurchaseUnits.First().ReferenceId);

        await UpdatePaymentRequestStatusAsync(paymentRequestId, order);
    }

    protected virtual async Task<PaymentRequest> UpdatePaymentRequestStatusAsync(Guid paymentRequestId, Order order)
    {
        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);

        if (order.Status is PayPalConsts.OrderStatus.Completed or PayPalConsts.OrderStatus.Approved)
        {
            paymentRequest.Complete();
        }
        else
        {
            paymentRequest.Failed(order.Status);
        }

        if (paymentRequest.Currency.IsNullOrEmpty())
        {
            paymentRequest.Currency = order.PurchaseUnits?.FirstOrDefault()?.Items.FirstOrDefault()?.UnitAmount?.CurrencyCode;
        }

        return await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        var orderId = properties["OrderId"]?.ToString();
        if (orderId.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty OrderId.");
        }

        var request = new OrdersGetRequest(orderId);
        var order = AsyncHelper.RunSync(() => PayPalHttpClient.Execute(request)).Result<Order>();

        if (!order.Status.Equals("COMPLETED", StringComparison.InvariantCulture))
        {
            throw new Exception("Order not completed.");
        }

        return order.PurchaseUnits.First().ReferenceId == paymentRequest.Id.ToString();
    }
}
