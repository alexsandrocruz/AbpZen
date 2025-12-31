using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Payment.Requests;

namespace Volo.Payment.TwoCheckout.Pages.Payment.TwoCheckout;

public class PostPaymentModel : PageModel
{
    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    public ILogger<PostPaymentModel> Logger { get; set; }

    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;

    public PostPaymentModel(
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentWebOptions)
    {
        PaymentRequestAppService = paymentRequestAppService;
        _paymentWebOptions = paymentWebOptions;
        Logger = NullLogger<PostPaymentModel>.Instance;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await _paymentWebOptions.SetAsync();
        
        var paymentRequestId = Request.Query["paymentRequestId"];

        Logger.LogInformation("2Checkout return url: " + Request.GetEncodedUrl());

        await PaymentRequestAppService.CompleteAsync(
            TwoCheckoutConsts.GatewayName,
            new()
            {
                { TwoCheckoutConsts.ParameterNames.PaymentRequestId, paymentRequestId },
                { "hmac-sha256", Request.Query["hmac-sha256"] }
            });

        Logger.LogDebug("Payment Success! " + "Payment ID: " + paymentRequestId + " Gateway Name: " + TwoCheckoutConsts.GatewayName);
        
        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var callbackUrl = _paymentWebOptions.Value.CallbackUrl + "?paymentRequestId=" + paymentRequestId;

            Response.Redirect(callbackUrl);
        }

        return Page();
    }

    public virtual IActionResult OnPost()
    {
        return BadRequest();
    }
}
