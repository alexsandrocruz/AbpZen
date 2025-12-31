using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Subscription;

namespace Volo.Saas.DemoWithPaymentApp.Pages;

public class IndexModel : PageModel
{
    public EditionDto Edition { get; set; }

    protected ISubscriptionAppService SubscriptionAppService { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public IndexModel(
        ISubscriptionAppService subscriptionAppService,
        ICurrentTenant currentTenant)
    {
        SubscriptionAppService = subscriptionAppService;
        CurrentTenant = currentTenant;
    }

    public async Task<IActionResult> OnPostAsync(Guid editionId)
    {
        var paymentRequest = await SubscriptionAppService.CreateSubscriptionAsync(editionId, CurrentTenant.GetId());

        return LocalRedirectPreserveMethod("/Payment/GatewaySelection?paymentRequestId=" + paymentRequest.Id);
    }
}
