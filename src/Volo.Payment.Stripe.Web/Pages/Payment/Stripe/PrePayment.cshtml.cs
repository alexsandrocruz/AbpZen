using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.Stripe.Pages.Payment.Stripe;

public class PrePaymentModel : AbpPageModel
{
    private static readonly Dictionary<PaymentType, string> modeMapping = new Dictionary<PaymentType, string>
        {
            { PaymentType.OneTime, "payment" },
            { PaymentType.Subscription, "subscription" }
        };

    [BindProperty] public Guid PaymentRequestId { get; set; }

    public string PublishableKey { get; set; }

    public string SessionId { get; set; }

    public PaymentRequestStartResultDto StartResult { get; protected set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    protected StripeWebOptions StripeWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    protected IPlanAppService PlanAppService { get; }

    public PrePaymentModel(
        IOptions<PaymentWebOptions> paymentWebOptions,
        IOptions<StripeWebOptions> stripeOptions,
        IPaymentRequestAppService paymentRequestAppService,
        IPlanAppService planAppService)
    {
        PaymentWebOptions = paymentWebOptions;
        StripeWebOptions = stripeOptions.Value;
        PaymentRequestAppService = paymentRequestAppService;
        PlanAppService = planAppService;
    }

    public virtual ActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual async Task OnPostAsync()
    {
        await PaymentWebOptions.SetAsync();
        
        StartResult = await PaymentRequestAppService.StartAsync(StripeConsts.GatewayName, new PaymentRequestStartDto
        {
            ReturnUrl = PaymentWebOptions.Value.RootUrl.RemovePostFix("/") + StripeConsts.PostPaymentUrl +
                         "?SessionId={CHECKOUT_SESSION_ID}",
            CancelUrl = PaymentWebOptions.Value.RootUrl,

            PaymentRequestId = PaymentRequestId
        });

        // TODO: Find better way that doesn't expose Publishable Key.
        // TODO: Checkout link should be built on Server-Side
        PublishableKey = StartResult.ExtraProperties[StripeConsts.ParameterNames.PublishableKey].ToString();
        SessionId = StartResult.ExtraProperties[StripeConsts.ParameterNames.SessionId].ToString();
    }
}
