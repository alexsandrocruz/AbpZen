using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Payment.Plans;

public interface IPlanRepository : IBasicRepository<Plan, Guid>
{
    Task<List<Plan>> GetPagedAndFilteredListAsync(int skipCount, int maxResultCount, string sorting, string filter, bool includeDetails = false, CancellationToken cancellationToken = default);
    Task<int> GetFilteredCountAsync(string filter, CancellationToken cancellationToken = default);
    Task<List<Plan>> GetManyAsync(Guid[] ids);
    Task<GatewayPlan> GetGatewayPlanAsync(Guid planId, string gateway);
    Task InsertGatewayPlanAsync(GatewayPlan gatewayPlan);
    Task DeleteGatewayPlanAsync(Guid planId, string gateway);
    Task UpdateGatewayPlanAsync(GatewayPlan gatewayPlan);
    Task<List<GatewayPlan>> GetGatewayPlanPagedListAsync(Guid planId, int skipCount, int maxResultCount, string sorting, string filter = null);
    Task<int> GetGatewayPlanCountAsync(Guid planId, string filter = null);
}
