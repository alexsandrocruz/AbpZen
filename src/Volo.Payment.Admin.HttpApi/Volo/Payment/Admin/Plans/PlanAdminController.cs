using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Plans;

[RemoteService(Name = AbpPaymentAdminRemoteServiceConsts.RemoteServiceName)]
[Area(AbpPaymentAdminRemoteServiceConsts.ModuleName)]
[Authorize(PaymentAdminPermissions.Plans.Default)]
[Route("api/payment-admin/plans")]
public class PlanAdminController : PaymentAdminController, IPlanAdminAppService
{
    protected IPlanAdminAppService PlanAdminAppService { get; }

    public PlanAdminController(IPlanAdminAppService planAdminAppService)
    {
        PlanAdminAppService = planAdminAppService;
    }

    [HttpPost]
    [Authorize(PaymentAdminPermissions.Plans.Create)]
    public virtual Task<PlanDto> CreateAsync(PlanCreateInput input)
    {
        return PlanAdminAppService.CreateAsync(input);
    }

    [HttpPost]
    [Authorize(PaymentAdminPermissions.Plans.Update)]
    [Route("{planId}/external-plans")]
    public virtual Task CreateGatewayPlanAsync(Guid planId, GatewayPlanCreateInput input)
    {
        return PlanAdminAppService.CreateGatewayPlanAsync(planId, input);
    }

    [HttpDelete]
    [Authorize(PaymentAdminPermissions.Plans.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return PlanAdminAppService.DeleteAsync(id);
    }

    [HttpDelete]
    [Authorize(PaymentAdminPermissions.Plans.Update)]
    [Route("{planId}/external-plans/{gateway}")]
    public virtual Task DeleteGatewayPlanAsync(Guid planId, string gateway)
    {
        return PlanAdminAppService.DeleteGatewayPlanAsync(planId, gateway);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<PlanDto> GetAsync(Guid id)
    {
        return PlanAdminAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<PlanDto>> GetListAsync(PlanGetListInput input)
    {
        return PlanAdminAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(PaymentAdminPermissions.Plans.Update)]
    public virtual Task<PlanDto> UpdateAsync(Guid id, PlanUpdateInput input)
    {
        return PlanAdminAppService.UpdateAsync(id, input);
    }

    [HttpPut]
    [Route("{planId}/external-plans/{gateway}")]
    [Authorize(PaymentAdminPermissions.Plans.Update)]
    public virtual Task UpdateGatewayPlanAsync(Guid planId, string gateway, GatewayPlanUpdateInput input)
    {
        return PlanAdminAppService.UpdateGatewayPlanAsync(planId, gateway, input);
    }

    [HttpGet("{planId}/external-plans")]
    public virtual Task<PagedResultDto<GatewayPlanDto>> GetGatewayPlansAsync(Guid planId, GatewayPlanGetListInput input)
    {
        return PlanAdminAppService.GetGatewayPlansAsync(planId, input);
    }
}
