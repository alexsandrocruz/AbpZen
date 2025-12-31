using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Payment.Plans;

public class PlanAppService : PaymentAppServiceBase, IPlanAppService
{
    protected IPlanRepository PlanRepository { get; }

    public PlanAppService(IPlanRepository planRepository)
    {
        PlanRepository = planRepository;
    }

    public virtual async Task<GatewayPlanDto> GetGatewayPlanAsync(Guid planId, string gateway)
    {
        var gatewayPlan = await PlanRepository.GetGatewayPlanAsync(planId, gateway);

        return ObjectMapper.Map<GatewayPlan, GatewayPlanDto>(gatewayPlan);
    }

    public virtual async Task<List<PlanDto>> GetPlanListAsync()
    {
        var plans = await PlanRepository.GetListAsync();

        return ObjectMapper.Map<List<Plan>, List<PlanDto>>(plans);
    }

    public virtual async Task<PlanDto> GetAsync(Guid planId)
    {
        var plan = await PlanRepository.FindAsync(planId);

        if (plan is null)
        {
            return null;
        }

        return ObjectMapper.Map<Plan, PlanDto>(plan);
    }

    public virtual async Task<List<PlanDto>> GetManyAsync(Guid[] planIds)
    {
        var plans = await PlanRepository.GetManyAsync(planIds);

        return ObjectMapper.Map<List<Plan>, List<PlanDto>>(plans);
    }
}
