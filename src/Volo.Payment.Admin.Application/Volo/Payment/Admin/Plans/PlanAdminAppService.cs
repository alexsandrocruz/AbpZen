using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Plans;

[Authorize(PaymentAdminPermissions.Plans.Default)]
public class PlanAdminAppService : PaymentAdminAppServiceBase, IPlanAdminAppService
{
    protected IPlanRepository PlanRepository { get; }

    public PlanAdminAppService(IPlanRepository planRepository)
    {
        PlanRepository = planRepository;
    }

    [Authorize(PaymentAdminPermissions.Plans.GatewayPlans.Delete)]
    public virtual async Task DeleteGatewayPlanAsync(Guid planId, string gateway)
    {
        await PlanRepository.DeleteGatewayPlanAsync(planId, gateway);
    }

    [Authorize(PaymentAdminPermissions.Plans.GatewayPlans.Create)]
    public virtual async Task CreateGatewayPlanAsync(Guid planId, GatewayPlanCreateInput input)
    {
        var gatewayPlan = new GatewayPlan(planId, input.Gateway, input.ExternalId);

        input.MapExtraPropertiesTo(gatewayPlan);

        await PlanRepository.InsertGatewayPlanAsync(gatewayPlan);
    }

    [Authorize(PaymentAdminPermissions.Plans.GatewayPlans.Update)]
    public virtual async Task UpdateGatewayPlanAsync(Guid planId, string gateway, GatewayPlanUpdateInput input)
    {
        var gatewayPlan = await PlanRepository.GetGatewayPlanAsync(planId, gateway);
        gatewayPlan.SetExternalId(input.ExternalId);

        input.MapExtraPropertiesTo(gatewayPlan);

        await PlanRepository.UpdateGatewayPlanAsync(gatewayPlan);
    }

    public virtual async Task<PlanDto> GetAsync(Guid id)
    {
        var plan = await PlanRepository.GetAsync(id);

        return ObjectMapper.Map<Plan, PlanDto>(plan);
    }

    public virtual async Task<PagedResultDto<PlanDto>> GetListAsync(PlanGetListInput input)
    {
        var plans = await PlanRepository.GetPagedAndFilteredListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting.IsNullOrEmpty() ? "Id desc" : input.Sorting,
            input.Filter,
            includeDetails: true);

        var count = await PlanRepository.GetFilteredCountAsync(input.Filter);

        return new PagedResultDto<PlanDto>(count, ObjectMapper.Map<List<Plan>, List<PlanDto>>(plans));
    }

    [Authorize(PaymentAdminPermissions.Plans.Create)]
    public virtual async Task<PlanDto> CreateAsync(PlanCreateInput input)
    {
        var plan = new Plan(GuidGenerator.Create(), input.Name);

        input.MapExtraPropertiesTo(plan);

        await PlanRepository.InsertAsync(plan);

        return ObjectMapper.Map<Plan, PlanDto>(plan);
    }

    [Authorize(PaymentAdminPermissions.Plans.Update)]
    public virtual async Task<PlanDto> UpdateAsync(Guid id, PlanUpdateInput input)
    {
        var plan = await PlanRepository.GetAsync(id);

        plan.Name = input.Name;
        plan.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        input.MapExtraPropertiesTo(plan);

        await PlanRepository.UpdateAsync(plan);

        return ObjectMapper.Map<Plan, PlanDto>(plan);
    }

    [Authorize(PaymentAdminPermissions.Plans.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return PlanRepository.DeleteAsync(id);
    }

    [Authorize(PaymentAdminPermissions.Plans.GatewayPlans.Default)]
    public virtual async Task<PagedResultDto<GatewayPlanDto>> GetGatewayPlansAsync(Guid planId, GatewayPlanGetListInput input)
    {
        var gatewayPlans = await PlanRepository.GetGatewayPlanPagedListAsync(planId, input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);

        var count = await PlanRepository.GetGatewayPlanCountAsync(planId, input.Filter);

        return new PagedResultDto<GatewayPlanDto>(
            count,
            ObjectMapper.Map<ICollection<GatewayPlan>, List<GatewayPlanDto>>(gatewayPlans));
    }
}
