using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Payment.Requests;

namespace Volo.Payment.Payu.Pages.Payment.Payu;

public class PostPaymentModel : PageModel
{
    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    public ILogger<PostPaymentModel> Logger { get; set; }

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
        Logger.LogInformation("PayU return url: " + Request.GetEncodedUrl());

        await _paymentWebOptions.SetAsync();
        
        var paymentRequestId = Request.Query["paymentRequestId"];

        await PaymentRequestAppService.CompleteAsync(
            PayuConsts.GatewayName,
            new()
            {
                { PayuConsts.ParameterNames.PaymentRequestId, paymentRequestId },
                { PayuConsts.ParameterNames.Url, GetCurrentEncodedUrl() },
                { PayuConsts.ParameterNames.PayRefNo, Request.Query["payrefno"] },
                { PayuConsts.ParameterNames.Ctrl, Request.Query["ctrl"] },
            });

        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var callbackUrl = _paymentWebOptions.Value.CallbackUrl + "?paymentRequestId=" + paymentRequestId;

            Response.Redirect(callbackUrl);
        }

        return Page();
    }

    private string GetCurrentEncodedUrl()
    {
        // changes Scheme of return URL with the Scheme of the callback URL.
        var originatedScheme = new Uri(_paymentWebOptions.Value.CallbackUrl).Scheme;
        return UriHelper.BuildAbsolute(originatedScheme, Request.Host, Request.PathBase, Request.Path, Request.QueryString);
    }

    public virtual IActionResult OnPost()
    {
        return BadRequest();
    }
}
