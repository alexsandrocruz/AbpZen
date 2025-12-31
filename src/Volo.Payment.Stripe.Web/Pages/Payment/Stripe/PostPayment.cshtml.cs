using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Payment.Requests;

namespace Volo.Payment.Stripe.Pages.Payment.Stripe;

public class PostPaymentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SessionId { get; set; }

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
        if (SessionId.IsNullOrWhiteSpace())
        {
            return BadRequest();
        }
        
        await _paymentWebOptions.SetAsync();

        var paymentRequest = await PaymentRequestAppService.CompleteAsync(
            StripeConsts.GatewayName,
            new()
            {
                { StripeConsts.ParameterNames.SessionId, SessionId },
            });

        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var callbackUrl = _paymentWebOptions.Value.CallbackUrl + "?paymentRequestId=" + paymentRequest.Id;

            // TODO: Find a way appending parameters from purchaseParameterListGenerator
            //if (!extraPaymentParameters.AdditionalCallbackParameters.IsNullOrEmpty())
            //{
            //    callbackUrl += "&" + extraPaymentParameters.AdditionalCallbackParameters;
            //}

            Response.Redirect(callbackUrl);
        }

        return Page();
    }


    public virtual IActionResult OnPost()
    {
        return BadRequest();
    }
}
