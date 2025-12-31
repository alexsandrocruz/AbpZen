using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Payment.Plans;

public interface IPlanAppService : IApplicationService
{
    Task<GatewayPlanDto> GetGatewayPlanAsync(Guid planId, string gateway);
    Task<List<PlanDto>> GetPlanListAsync();
    Task<PlanDto> GetAsync(Guid planId);
    Task<List<PlanDto>> GetManyAsync(Guid[] planIds);
}
