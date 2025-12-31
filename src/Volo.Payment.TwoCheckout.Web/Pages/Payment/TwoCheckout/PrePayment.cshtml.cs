using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Requests;

namespace Volo.Payment.TwoCheckout.Pages.Payment.TwoCheckout;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty]
    public Guid PaymentRequestId { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected TwoCheckoutWebOptions TwoCheckoutWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    protected IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    public PrePaymentModel(
        IOptions<TwoCheckoutWebOptions> twoCheckoutOptions,
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentWebOptions)
    {
        PaymentRequestAppService = paymentRequestAppService;
        TwoCheckoutWebOptions = twoCheckoutOptions.Value;
        PaymentWebOptions = paymentWebOptions;
    }

    public virtual ActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual async Task OnPostAsync()
    {
        await PaymentWebOptions.SetAsync();
        
        PaymentRequest = await PaymentRequestAppService.GetAsync(PaymentRequestId);

        var resultDto = await PaymentRequestAppService.StartAsync(
            TwoCheckoutConsts.GatewayName,
            new PaymentRequestStartDto
            {
                PaymentRequestId = PaymentRequestId,
                ReturnUrl = PaymentWebOptions.Value.Gateways[TwoCheckoutConsts.GatewayName].PostPaymentUrl +
                         "?paymentRequestId=" + PaymentRequestId
            });

        Response.Redirect(resultDto.CheckoutLink);
    }
}
