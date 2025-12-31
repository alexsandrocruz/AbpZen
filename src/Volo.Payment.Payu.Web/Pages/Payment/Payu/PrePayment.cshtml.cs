using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Requests;

namespace Volo.Payment.Payu.Pages.Payment.Payu;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty] public Guid PaymentRequestId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string PrePaymentCheckoutButtonStyle { get; set; }

    public PaymentRequestStartResultDto PaymentRequestStartResult { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }
    protected PayuWebOptions PayuWebOptions { get; }
    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    protected IOptions<PaymentWebOptions> PaymentGatewayOptions { get; }


    public PrePaymentModel(
        IOptions<PaymentWebOptions> paymentGatewayOptions,
        IOptions<PayuWebOptions> payuOptions,
        IPaymentRequestAppService paymentRequestAppService)
    {
        PaymentRequestAppService = paymentRequestAppService;
        PayuWebOptions = payuOptions.Value;
        PaymentGatewayOptions = paymentGatewayOptions;
    }

    public virtual ActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual async Task OnPostAsync()
    {
        if (CurrentUser.Id != null)
        {
            Name = CurrentUser.Name;
            Surname = CurrentUser.SurName;
            Email = CurrentUser.Email;
        }
        
        await PaymentGatewayOptions.SetAsync();

        PrePaymentCheckoutButtonStyle = PayuWebOptions.PrePaymentCheckoutButtonStyle;
        PaymentRequest = await PaymentRequestAppService.GetAsync(PaymentRequestId);

        var postPaymentUrl = PaymentGatewayOptions.Value.Gateways[PayuConsts.GatewayName].PostPaymentUrl;

        PaymentRequestStartResult = await PaymentRequestAppService.StartAsync(PayuConsts.GatewayName, new PaymentRequestStartDto
        {
            ReturnUrl = postPaymentUrl + "?paymentRequestId=" + PaymentRequestId,
            PaymentRequestId = PaymentRequestId,
        });
    }
}
