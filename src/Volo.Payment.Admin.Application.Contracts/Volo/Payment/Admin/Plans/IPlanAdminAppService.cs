using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Plans;

public interface IPlanAdminAppService : ICrudAppService<PlanDto, Guid, PlanGetListInput, PlanCreateInput, PlanUpdateInput>
{
    Task<PagedResultDto<GatewayPlanDto>> GetGatewayPlansAsync(Guid planId, GatewayPlanGetListInput input);

    Task DeleteGatewayPlanAsync(Guid planId, string gateway);

    Task CreateGatewayPlanAsync(Guid planId, GatewayPlanCreateInput input);

    Task UpdateGatewayPlanAsync(Guid planId, string gateway, GatewayPlanUpdateInput input);
}
