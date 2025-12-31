using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Payment.Requests;
using Volo.Saas.Subscription;

namespace Volo.Saas.Host;

[Controller]
[RemoteService(Name = SaasHostRemoteServiceConsts.RemoteServiceName)]
[Authorize(SaasHostPermissions.Editions.Default)]
[Area(SaasHostRemoteServiceConsts.ModuleName)]
[ControllerName("Edition")]
[Route("/api/saas/subscription")]
public class SubscriptionController : AbpControllerBase, ISubscriptionAppService
{
    protected ISubscriptionAppService SubscriptionAppService { get; }

    public SubscriptionController(ISubscriptionAppService subscriptionAppService)
    {
        SubscriptionAppService = subscriptionAppService;
    }

    [HttpPost]
    public Task<PaymentRequestWithDetailsDto> CreateSubscriptionAsync(Guid editionId, Guid tenantId)
    {
        return SubscriptionAppService.CreateSubscriptionAsync(editionId, tenantId);
    }
}
