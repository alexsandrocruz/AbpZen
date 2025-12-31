using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using Volo.Payment.Gateways;
using Volo.Payment.Plans;
using Volo.Payment.Requests;
using Volo.Payment.Subscription;

namespace Volo.Payment.Stripe;

public class StripePaymentGateway : IPaymentGateway, ITransientDependency
{
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }

    protected IPlanRepository PlanRepository { get; }

    protected StripeOptions StripeOptions { get; }

    protected ILogger<StripePaymentGateway> Logger { get; }

    protected IDistributedEventBus EventBus { get; }

    protected IObjectMapper ObjectMapper { get; }

    protected readonly static Dictionary<PaymentType, string> modeMapping = new Dictionary<PaymentType, string>
    {
        { PaymentType.OneTime, "payment" },
        { PaymentType.Subscription, "subscription" }
    };

    public StripePaymentGateway(
        IPaymentRequestRepository paymentRequestRepository,
        IPurchaseParameterListGenerator purchaseParameterListGenerator,
        IPlanRepository planRepository,
        IOptions<StripeOptions> stripeOptions,
        ILogger<StripePaymentGateway> logger,
        IDistributedEventBus eventBus,
        IObjectMapper objectMapper)
    {
        PaymentRequestRepository = paymentRequestRepository;
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        PlanRepository = planRepository;
        StripeOptions = stripeOptions.Value;
        Logger = logger;
        EventBus = eventBus;
        ObjectMapper = objectMapper;
    }

    public virtual async Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var purchaseParameters = PurchaseParameterListGenerator.GetExtraParameterConfiguration(paymentRequest);

        var currency = paymentRequest.Currency.IsNullOrEmpty() ? purchaseParameters.Currency : paymentRequest.Currency;

        var lineItems = new List<SessionLineItemOptions>();

        foreach (var product in paymentRequest.Products)
        {
            var lineItem = new SessionLineItemOptions
            {
                Quantity = product.Count,
            };

            if (product.PaymentType == PaymentType.Subscription)
            {
                var gatewayPlan = await PlanRepository.GetGatewayPlanAsync(product.PlanId.Value, StripeConsts.GatewayName);
                lineItem.Price = gatewayPlan.ExternalId;
            }

            if (product.PaymentType == PaymentType.OneTime)
            {
                lineItem.PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = Convert.ToDecimal(product.UnitPrice) * 100,
                    Currency = currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                        Metadata = new Dictionary<string, string>
                            {
                                { "ProductCode", product.Code }
                            }
                    }
                };
            }

            lineItems.Add(lineItem);
        }

        var options = new SessionCreateOptions
        {
            Locale = purchaseParameters.Locale,
            PaymentMethodTypes = purchaseParameters.PaymentMethodTypes,
            LineItems = lineItems,

            Mode = modeMapping[paymentRequest.Products.First().PaymentType],

            SuccessUrl = input.ReturnUrl,
            CancelUrl = input.CancelUrl,

            Metadata = new Dictionary<string, string>
            {
                { StripeConsts.ParameterNames.PaymentRequestId, paymentRequest.Id.ToString()}
            }
        };

        var sessionService = new SessionService();
        var session = await sessionService.CreateAsync(options);

        return new PaymentRequestStartResult
        {
            CheckoutLink = "https://js.stripe.com/v3/",
            ExtraProperties =
            {
                { StripeConsts.ParameterNames.PublishableKey, StripeOptions.PublishableKey},
                { StripeConsts.ParameterNames.SessionId, session.Id}
            }
        };
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(parameters[StripeConsts.ParameterNames.SessionId]);

        Logger.LogInformation("Stripe session object: " + session.ToJson());

        var paymentRequestId = Guid.Parse(session.Metadata["PaymentRequestId"]);
        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);
        if (paymentRequest.Currency.IsNullOrEmpty())
        {
            paymentRequest.Currency = session.Currency;
        }

        if (IsValid(paymentRequest, session))
        {
            paymentRequest.Complete();
        }
        else
        {
            paymentRequest.Failed();
        }

        if (session.SubscriptionId != null)
        {
            var subscription = await new SubscriptionService().GetAsync(session.SubscriptionId);

            paymentRequest.SetExternalSubscriptionId(session.SubscriptionId);
            paymentRequest.Currency = subscription.Items.FirstOrDefault()?.Price.Currency;

            // TODO: Remove this event from here. It should be published from appservice.
            var subscriptionCreatedEto = ObjectMapper.Map<PaymentRequest, SubscriptionCreatedEto>(paymentRequest);
            subscriptionCreatedEto.PeriodEndDate = subscription.CurrentPeriodEnd;

            await EventBus.PublishAsync(subscriptionCreatedEto);
        }

        return await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    public virtual async Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        var signature = headers.FirstOrDefault(x => x.Key.Equals("Stripe-Signature", StringComparison.OrdinalIgnoreCase)).Value;
        var stripeEvent = EventUtility.ConstructEvent(
            payload,
            signature,
            StripeOptions.WebhookSecret,
            throwOnApiVersionMismatch: false
        );

        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                {
                    await HandleCheckoutSessionCompletedAsync(stripeEvent);
                    break;
                }
            case EventTypes.CustomerSubscriptionDeleted:
                {
                    await HandleCustomerSubscriptionDeletedAsync(stripeEvent);
                    break;
                }
            case EventTypes.CustomerSubscriptionUpdated:
                {
                    await HandleCustomerSubscriptionUpdatedAsync(stripeEvent);
                    break;
                }
        }
    }

    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        var sessionId = properties["SessionId"];
        if (sessionId.IsNullOrWhiteSpace())
        {
            throw new Exception("Empty SessionId.");
        }

        var sessionService = new SessionService();
        var session = sessionService.Get(sessionId);

        if (!session.PaymentStatus.Equals("paid", StringComparison.InvariantCulture))
        {
            throw new Exception("Session not paid.");
        }

        return session.Metadata["PaymentRequestId"] == paymentRequest.Id.ToString();
    }

    protected virtual bool IsValid(PaymentRequest paymentRequest, Session session)
    {
        if (!session.PaymentStatus.Equals("paid", StringComparison.InvariantCulture))
        {
            throw new Exception("Session not paid.");
        }

        return session.Metadata[StripeConsts.ParameterNames.PaymentRequestId] == paymentRequest.Id.ToString();
    }

    protected virtual async Task HandleCheckoutSessionCompletedAsync(Event stripeEvent)
    {
        var paymentRequestId = Guid.Parse((string)stripeEvent.Data.RawObject.metadata["PaymentRequestId"]?.ToString());
        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);

        string subscriptionId = stripeEvent.Data.RawObject.subscription?.ToString();

        if (subscriptionId != null)
        {
            paymentRequest.SetExternalSubscriptionId(subscriptionId);

            var subscription = await new SubscriptionService().GetAsync(subscriptionId);

            var subscriptionCreatedEto = ObjectMapper.Map<PaymentRequest, SubscriptionCreatedEto>(paymentRequest);
            subscriptionCreatedEto.PeriodEndDate = subscription.CurrentPeriodEnd;

            await EventBus.PublishAsync(subscriptionCreatedEto);
        }

        paymentRequest.Complete();
        await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    protected virtual async Task HandleCustomerSubscriptionUpdatedAsync(Event stripeEvent)
    {
        var paymentRequest =
            await PaymentRequestRepository.GetBySubscriptionAsync(
                (string)stripeEvent.Data.RawObject.id);

        var paymentUpdatedEto =
            ObjectMapper.Map<PaymentRequest, SubscriptionUpdatedEto>(paymentRequest);

        paymentUpdatedEto.PeriodEndDate =
            ConvertToDateTime((int)stripeEvent.Data.RawObject.current_period_end);

        await EventBus.PublishAsync(paymentUpdatedEto);
    }

    protected virtual async Task HandleCustomerSubscriptionDeletedAsync(Event stripeEvent)
    {
        var paymentRequest =
            await PaymentRequestRepository.GetBySubscriptionAsync(
                (string)stripeEvent.Data.RawObject.id);

        var subscriptionCanceledEto =
            ObjectMapper.Map<PaymentRequest, SubscriptionCanceledEto>(paymentRequest);

        subscriptionCanceledEto.PeriodEndDate =
            ConvertToDateTime((int)stripeEvent.Data.RawObject.current_period_end);

        await EventBus.PublishAsync(subscriptionCanceledEto);
    }

    protected virtual DateTime ConvertToDateTime(int unixSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime;
    }
}
