using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;

namespace Volo.Payment.Pages.Payment;

public class GatewaySelectionModel : PaymentPageModel
{
    [BindProperty]
    public Guid PaymentRequestId { get; set; }

    public List<PaymentGatewayWebConfiguration> Gateways { get; set; }

    public string CheckoutButtonStyle { get; set; }

    private readonly IPaymentRequestAppService _paymentRequestAppService;
    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;
    private readonly IGatewayAppService _gatewayAppService;
    public GatewaySelectionModel(
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentWebOptions,
        IGatewayAppService gatewayAppService)
    {
        _paymentRequestAppService = paymentRequestAppService;
        _paymentWebOptions = paymentWebOptions;
        _gatewayAppService = gatewayAppService;
    }

    public virtual ActionResult OnGet()
    {
        return BadRequest();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await _paymentWebOptions.SetAsync();
        
        CheckoutButtonStyle = _paymentWebOptions.Value.GatewaySelectionCheckoutButtonStyle;

        var paymentRequest = await _paymentRequestAppService.GetAsync(PaymentRequestId);

        List<GatewayDto> gatewaysDtos;

        if (paymentRequest.Products.Any(a => a.PaymentType == PaymentType.Subscription))
        {
            gatewaysDtos = await _gatewayAppService.GetSubscriptionSupportedGatewaysAsync();
        }
        else
        {
            gatewaysDtos = await _gatewayAppService.GetGatewayConfigurationAsync();
        }

        Gateways = _paymentWebOptions.Value.Gateways
            .Where(x => gatewaysDtos.Any(a => a.Name == x.Key))
            .Select(x => x.Value)
            .ToList();

        if (!Gateways.Any())
        {
            throw new ApplicationException("No payment gateway configured!");
        }

        if (Gateways.Count == 1)
        {
            var gateway = Gateways.First();
            return LocalRedirectPreserveMethod(gateway.PrePaymentUrl + "?paymentRequestId=" + PaymentRequestId);
        }

        return Page();
    }
}
