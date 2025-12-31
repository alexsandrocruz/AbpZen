using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Requests;

namespace Volo.Payment.Alipay.Pages.Payment.Alipay;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid PaymentRequestId { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    
    public string Body { get; set; }

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

        var response = await PaymentRequestAppService.StartAsync(AlipayConsts.GatewayName, new PaymentRequestStartDto
        {
            PaymentRequestId = PaymentRequestId,
            ReturnUrl = PaymentWebOptions.Value.RootUrl.RemovePostFix("/") + AlipayConsts.PostPaymentUrl,
            CancelUrl = PaymentWebOptions.Value.RootUrl,
        });

        Body = response.CheckoutLink;
        return Page();
    }
}
