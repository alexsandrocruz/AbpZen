using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Payment.Requests;

namespace Volo.Payment.WeChatPay.Pages.Payment.WeChatPay;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid PaymentRequestId { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    
    protected IWebClientInfoProvider WebClientInfoProvider { get; }
    
    public string CheckoutLink { get; set; }

    public PrePaymentModel(
        IOptions<PaymentWebOptions> paymentWebOptions,
        IPaymentRequestAppService paymentRequestAppService)
    {
        PaymentWebOptions = paymentWebOptions;
        PaymentRequestAppService = paymentRequestAppService;
    }

    public virtual IActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await PaymentWebOptions.SetAsync();
        
        PaymentRequest = await PaymentRequestAppService.GetAsync(PaymentRequestId);

        var paymentRequestStartDto = new PaymentRequestStartDto 
        {
            PaymentRequestId = PaymentRequestId,
            ReturnUrl = PaymentWebOptions.Value.RootUrl.RemovePostFix("/") + WeChatPayConsts.PostPaymentUrl,
            CancelUrl = PaymentWebOptions.Value.RootUrl,
            ExtraProperties = { [WeChatPaymentParameterConsts.PayerIp] = WebClientInfoProvider.ClientIpAddress }
        };

        var response = await PaymentRequestAppService.StartAsync(WeChatPayConsts.GatewayName, paymentRequestStartDto);

        CheckoutLink = response.CheckoutLink;

        return Page();
    }
}
