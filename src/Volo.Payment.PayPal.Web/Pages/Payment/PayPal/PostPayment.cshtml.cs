using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Payment.PayPal;
using Volo.Payment.Requests;

namespace Volo.Payment.Paypal.Pages.Payment.PayPal;

public class PostPaymentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

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
        if (Token.IsNullOrWhiteSpace())
        {
            return BadRequest();
        }
        
        await _paymentWebOptions.SetAsync();

        var paymentRequest = await PaymentRequestAppService.CompleteAsync(
            PayPalConsts.GatewayName,
            new()
            {
                { "Token", Token } // TODO: Magic string
            });

        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var callbackUrl = _paymentWebOptions.Value.CallbackUrl + "?paymentRequestId=" + paymentRequest.Id;

            Response.Redirect(callbackUrl);
        }

        return Page();
    }

    public virtual IActionResult OnPost()
    {
        return BadRequest();
    }
}
