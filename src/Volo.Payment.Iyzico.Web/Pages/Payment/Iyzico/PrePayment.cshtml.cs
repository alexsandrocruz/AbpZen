using System;
using System.Threading.Tasks;
using Iyzipay.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Payment.Requests;

namespace Volo.Payment.Iyzico.Pages.Payment.Iyzico;

public class PrePaymentModel : AbpPageModel
{
    [BindProperty] public Guid PaymentRequestId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string PrePaymentCheckoutButtonStyle { get; set; }

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IyzicoWebOptions IyzicoWebOptions { get; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }

    [BindProperty] public IyzicoCustomer Customer { get; set; }

    private readonly IOptions<PaymentWebOptions> _paymentGatewayOptions;

    public PrePaymentModel(
        IOptions<IyzicoWebOptions> iyzicoOptions,
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentGatewayOptions1)
    {
        PaymentRequestAppService = paymentRequestAppService;
        _paymentGatewayOptions = paymentGatewayOptions1;
        IyzicoWebOptions = iyzicoOptions.Value;

        Customer = new IyzicoCustomer();
    }

    public virtual ActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual Task OnPostAsync()
    {
        if (CurrentUser.Id != null)
        {
            Name = CurrentUser.Name;
            Surname = CurrentUser.SurName;
            Email = CurrentUser.Email;
        }

        PrePaymentCheckoutButtonStyle = IyzicoWebOptions.PrePaymentCheckoutButtonStyle;

        return Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostContinueToCheckout()
    {
        await _paymentGatewayOptions.SetAsync();
        
        PaymentRequest = await PaymentRequestAppService.GetAsync(PaymentRequestId);

        var callbackUrl = _paymentGatewayOptions.Value.Gateways[IyzicoConsts.GatewayName].PostPaymentUrl +
                       "?paymentRequestId=" + PaymentRequest.Id;

        var paymentRequestStartDto = new PaymentRequestStartDto
        {
            ReturnUrl = callbackUrl,
            PaymentRequestId = PaymentRequestId,
        };

        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.Name), Customer.Name);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.Surname), Customer.Surname);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.Email), Customer.Email);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.IdentityNumber), Customer.IdentityNumber);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.RegistrationAddress), Customer.Address.Description);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.Ip), Customer.IpAddress);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.City), Customer.Address.City);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.Country), Customer.Address.Country);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Buyer.ZipCode), Customer.Address.ZipCode);
        paymentRequestStartDto.ExtraProperties.Add(nameof(Address.Description), Customer.Address.Description);

        var response = await PaymentRequestAppService.StartAsync(IyzicoConsts.GatewayName, paymentRequestStartDto);

        return Redirect(response.CheckoutLink);
    }
}
