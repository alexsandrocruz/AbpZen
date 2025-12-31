using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Payment.Requests;

namespace Volo.Payment.Alipay.Pages.Payment.Alipay;

[IgnoreAntiforgeryToken]
public class PostPaymentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    public ILogger<PostPaymentModel> Logger { get; set; }
    
    [BindProperty(SupportsGet = true, Name = "out_trade_no")]
    public string OutTradeNo { get; set; }

    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;

    public PostPaymentModel(
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentWebOptions)
    {
        PaymentRequestAppService = paymentRequestAppService;
        _paymentWebOptions = paymentWebOptions;
        Logger = NullLogger<PostPaymentModel>.Instance;
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return CompleteAsync(new Dictionary<string, string>{{"out_trade_no", OutTradeNo}});
    }


    public virtual Task<IActionResult> OnPostAsync(IFormCollection formCollection)
    {
        return CompleteAsync(formCollection.Keys.ToDictionary(k => k, v => formCollection[v].ToString()));
    }

    protected virtual async Task<IActionResult> CompleteAsync(Dictionary<string, string> parameters)
    {
        await _paymentWebOptions.SetAsync();
        
        var paymentRequest = await PaymentRequestAppService.CompleteAsync(AlipayConsts.GatewayName, parameters);

        if (paymentRequest == null)
        {
            return BadRequest();
        }
        
        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var callbackUrl = _paymentWebOptions.Value.CallbackUrl + "?paymentRequestId=" + paymentRequest.Id;
            Response.Redirect(callbackUrl);
        }
        return Page();
    }
}
