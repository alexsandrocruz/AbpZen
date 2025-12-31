using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Requests;

namespace Volo.Payment.Iyzico.Pages.Payment.Iyzico;

[IgnoreAntiforgeryToken]
public class PostPaymentModel : AbpPageModel
{
    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    public ILogger<PostPaymentModel> Logger { get; set; }

    [BindProperty]
    public string ErrorMessage { get; set; }

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
        return await Task.FromResult(BadRequest());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _paymentWebOptions.SetAsync();
        Logger.LogInformation("Iyzico return url: " + Request.GetEncodedUrl());

        try
        {
            await PaymentRequestAppService.CompleteAsync(IyzicoConsts.GatewayName, new()
            {
                { IyzicoParameterConsts.PaymentRequestId, Request.Query["paymentRequestId"] },
                { IyzicoParameterConsts.Token, Request.Form["token"] },
                { IyzicoParameterConsts.Url, GetCurrentEncodedUrl() }
            });
        }
        catch (Exception ex)
        {
            var failedPayment = await PaymentRequestAppService.GetAsync(Guid.Parse(Request.Query["paymentRequestId"]));
            Logger.LogDebug("Payment Failed! " + "Payment ID: " + failedPayment.Id + " Gateway Name: " + IyzicoConsts.GatewayName + " Fail Reason: " + failedPayment.FailReason);
            
            ErrorMessage = ex.Message;
            return Page();
        }
        
        Logger.LogDebug("Payment Success! " + "Payment ID: " + Request.Query["paymentRequestId"] + " Gateway Name: " + IyzicoConsts.GatewayName);

        if (!_paymentWebOptions.Value.CallbackUrl.IsNullOrWhiteSpace())
        {
            var paymentRequestId = Guid.Parse(Request.Query["paymentRequestId"]);
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
}
