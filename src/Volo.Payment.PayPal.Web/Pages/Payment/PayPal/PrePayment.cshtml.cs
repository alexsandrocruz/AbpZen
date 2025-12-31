using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.PayPal;
using Volo.Payment.Requests;

namespace Volo.Payment.Paypal.Pages.Payment.PayPal;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid PaymentRequestId { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

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

    public virtual async Task OnPostAsync()
    {
        await PaymentWebOptions.SetAsync();
        
        PaymentRequest = await PaymentRequestAppService.GetAsync(PaymentRequestId);

        var response = await PaymentRequestAppService.StartAsync(PayPalConsts.GatewayName, new PaymentRequestStartDto
        {
            PaymentRequestId = PaymentRequestId,
            ReturnUrl = PaymentWebOptions.Value.RootUrl.RemovePostFix("/") + PayPalConsts.PostPaymentUrl,
            CancelUrl = PaymentWebOptions.Value.RootUrl,
        });

        Response.Redirect(response.CheckoutLink);
    }
}
