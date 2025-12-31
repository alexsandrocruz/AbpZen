using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;
using System.Threading;
using PaymentResource = Volo.Payment.Localization.PaymentResource;

namespace Volo.Payment.Iyzico;

public class IyzicoPaymentGateway : IPaymentGateway, ITransientDependency
{
    protected IyzicoOptions IyzicoOptions { get; }

    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    public IyzicoPaymentGateway(
        IOptions<IyzicoOptions> iyzicoOptions,
        IPurchaseParameterListGenerator purchaseParameterListGenerator,
        IStringLocalizerFactory localizerFactory,
        IPaymentRequestRepository paymentRequestRepository)
    {
        IyzicoOptions = iyzicoOptions.Value;
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        LocalizerFactory = localizerFactory;
        PaymentRequestRepository = paymentRequestRepository;
    }

    public virtual Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var config = PurchaseParameterListGenerator.GetExtraParameterConfiguration(paymentRequest);
        var totalPrice = paymentRequest.Products.Sum(p => p.TotalPrice).ToString("0.00",CultureInfo.InvariantCulture);
        var callbackUrl = input.ReturnUrl;

        var request = new CreateCheckoutFormInitializeRequest
        {
            Locale = config.Locale,
            ConversationId = paymentRequest.Id.ToString(),
            Price = totalPrice,
            PaidPrice = totalPrice,
            Currency = paymentRequest.Currency.IsNullOrEmpty() ? config.Currency : paymentRequest.Currency,
            BasketId = Guid.NewGuid().ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),
            CallbackUrl = callbackUrl,
            EnabledInstallments = new List<int> { 1 },
            Buyer = new Buyer
            {
                Id = Guid.NewGuid().ToString(),
                Name = input.ExtraProperties.GetOrDefault(nameof(Buyer.Name))?.ToString(),
                Surname = input.ExtraProperties.GetOrDefault(nameof(Buyer.Surname))?.ToString(),
                Email = input.ExtraProperties.GetOrDefault(nameof(Buyer.Email))?.ToString(),
                IdentityNumber = input.ExtraProperties.GetOrDefault(nameof(Buyer.IdentityNumber))?.ToString(),
                RegistrationAddress = input.ExtraProperties.GetOrDefault(nameof(Buyer.RegistrationAddress))?.ToString(),
                Ip = input.ExtraProperties.GetOrDefault(nameof(Buyer.Ip))?.ToString(),
                City = input.ExtraProperties.GetOrDefault(nameof(Buyer.City))?.ToString(),
                Country = input.ExtraProperties.GetOrDefault(nameof(Buyer.Country))?.ToString(),
                ZipCode = input.ExtraProperties.GetOrDefault(nameof(Buyer.ZipCode))?.ToString()
            }
        };

        var address = new Address
        {
            ContactName = input.ExtraProperties.GetOrDefault(nameof(Buyer.Name))?.ToString(),
            City = input.ExtraProperties.GetOrDefault(nameof(Buyer.City))?.ToString(),
            Country = input.ExtraProperties.GetOrDefault(nameof(Buyer.Country))?.ToString(),
            Description = input.ExtraProperties.GetOrDefault(nameof(Address.Description))?.ToString(),
            ZipCode = input.ExtraProperties.GetOrDefault(nameof(Buyer.ZipCode))?.ToString()
        };

        request.ShippingAddress = address;
        request.BillingAddress = address;
        request.BasketItems = new List<BasketItem>();

        foreach (var product in paymentRequest.Products)
        {
            for (var i = 0; i < product.Count; i++)
            {
                request.BasketItems.Add(new BasketItem
                {
                    Id = product.Code,
                    Name = product.Name,
                    Category1 = "Software",
                    ItemType = BasketItemType.VIRTUAL.ToString(),
                    Price = product.UnitPrice.ToString("0.00", CultureInfo.InvariantCulture)
                });
            }
        }

        var checkoutFormInitialize = CheckoutFormInitialize.Create(request, new Iyzipay.Options
        {
            ApiKey = IyzicoOptions.ApiKey,
            SecretKey = IyzicoOptions.SecretKey,
            BaseUrl = IyzicoOptions.BaseUrl
        });

        if (!checkoutFormInitialize.ErrorMessage.IsNullOrEmpty())
        {
            throw new UserFriendlyException(checkoutFormInitialize.ErrorMessage);
        }

        return Task.FromResult(new PaymentRequestStartResult
        {
            CheckoutLink = checkoutFormInitialize.PaymentPageUrl
        });
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var paymentRequestId = Guid.Parse(parameters[IyzicoParameterConsts.PaymentRequestId]);

        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);
        if (paymentRequest.State == PaymentRequestState.Completed)
        {
            return paymentRequest;
        }

        if (IsValid(paymentRequest, parameters))
        {
            paymentRequest.Complete();
        }

        await PaymentRequestRepository.UpdateAsync(paymentRequest);

        return paymentRequest;
    }

    public virtual Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        var token = properties["token"]?.ToString();

        var request = new RetrieveCheckoutFormRequest
        {
            ConversationId = paymentRequest.Id.ToString(),
            Token = token
        };

        var checkoutForm = CheckoutForm.Retrieve(request, new Iyzipay.Options
        {
            ApiKey = IyzicoOptions.ApiKey,
            SecretKey = IyzicoOptions.SecretKey,
            BaseUrl = IyzicoOptions.BaseUrl
        });

        paymentRequest.Currency = checkoutForm.Currency;

        if (!checkoutForm.ErrorGroup.IsNullOrEmpty())
        {
            var errorMessage = new LocalizableString(typeof(PaymentResource), "Iyzico:" + checkoutForm.ErrorGroup);
            var localizedMessage = errorMessage.Localize(LocalizerFactory).Value;
            paymentRequest.Failed(localizedMessage);
            PaymentRequestRepository.UpdateAsync(paymentRequest);
            
            throw new UserFriendlyException(message: localizedMessage);
        }

        return checkoutForm.PaymentStatus == "SUCCESS";
    }
}
