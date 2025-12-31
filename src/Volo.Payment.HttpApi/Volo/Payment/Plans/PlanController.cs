using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Volo.Payment.Plans;

[RemoteService(Name = AbpPaymentCommonRemoteServiceConsts.RemoteServiceName)]
[Area(AbpPaymentCommonRemoteServiceConsts.ModuleName)]
[Route("api/payment/plans")]
public class PlanController : PaymentCommonController, IPlanAppService
{
    protected IPlanAppService PlanAppService { get; }

    public PlanController(IPlanAppService planAppService)
    {
        PlanAppService = planAppService;
    }

    [HttpGet]
    [Route("{planId}/{gateway}")]
    public virtual Task<GatewayPlanDto> GetGatewayPlanAsync(Guid planId, string gateway)
    {
        return PlanAppService.GetGatewayPlanAsync(planId, gateway);
    }

    [HttpGet]
    public virtual Task<List<PlanDto>> GetPlanListAsync()
    {
        return PlanAppService.GetPlanListAsync();
    }

    [HttpGet]
    [Route("{planId}")]
    public virtual Task<PlanDto> GetAsync(Guid planId)
    {
        return PlanAppService.GetAsync(planId);
    }

    [HttpGet]
    [Route("many")]
    public virtual Task<List<PlanDto>> GetManyAsync(Guid[] ids)
    {
        return PlanAppService.GetManyAsync(ids);
    }
}
