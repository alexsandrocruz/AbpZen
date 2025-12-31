using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Payment.Plans;
using System.Linq.Dynamic.Core;
using System.Threading;
using MongoDB.Driver.Linq;

namespace Volo.Payment.MongoDB.Plans;

public class MongoPlanRepository : MongoDbRepository<IPaymentMongoDbContext, Plan, Guid>, IPlanRepository
{
    public MongoPlanRepository(IMongoDbContextProvider<IPaymentMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<GatewayPlan> GetGatewayPlanAsync(Guid planId, string gateway)
    {
        var plan = await GetAsync(planId);

        return plan.GatewayPlans.FirstOrDefault(x => x.Gateway == gateway)
            ?? throw new EntityNotFoundException(typeof(GatewayPlan));
    }

    public virtual async Task InsertGatewayPlanAsync(GatewayPlan gatewayPlan)
    {
        var plan = await GetAsync(gatewayPlan.PlanId);

        plan.GatewayPlans.AddIfNotContains(gatewayPlan);

        await UpdateAsync(plan);
    }

    public virtual async Task DeleteGatewayPlanAsync(Guid planId, string gateway)
    {
        var plan = await GetAsync(planId);

        if (plan.GatewayPlans.Remove(plan.GatewayPlans.FirstOrDefault(x => x.Gateway == gateway)))
        {
            await UpdateAsync(plan);
        }
    }

    public virtual async Task UpdateGatewayPlanAsync(GatewayPlan gatewayPlan)
    {
        var plan = await GetAsync(gatewayPlan.PlanId);

        if (plan.GatewayPlans.Remove(plan.GatewayPlans.FirstOrDefault(x => x.Gateway == gatewayPlan.Gateway)))
        {
            plan.GatewayPlans.Add(gatewayPlan);
            await UpdateAsync(plan);
        }
    }

    public virtual async Task<List<GatewayPlan>> GetGatewayPlanPagedListAsync(Guid planId, int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var queryable = (await GetQueryableAsync())
            .Where(x => x.Id == planId)
            .SelectMany(s => s.GatewayPlans)
            .WhereIf(!filter.IsNullOrEmpty(), x => x.Gateway.ToLower().Contains(filter) || x.ExternalId.ToLower().Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(GatewayPlan.Gateway) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await AsyncExecuter.ToListAsync(queryable);
    }

    public virtual async Task<int> GetGatewayPlanCountAsync(Guid planId, string filter = null)
    {
        var queryable = (await GetQueryableAsync())
            .Where(x => x.Id == planId)
            .SelectMany(s => s.GatewayPlans)
            .WhereIf(!filter.IsNullOrEmpty(), x => x.Gateway.ToLower().Contains(filter) || x.ExternalId.ToLower().Contains(filter));

        return await AsyncExecuter.CountAsync(queryable);
    }

    public virtual async Task<List<Plan>> GetManyAsync(Guid[] ids)
    {
        return await AsyncExecuter.ToListAsync((await GetMongoQueryableAsync()).Where(x => ids.Contains(x.Id)));
    }

    public virtual async Task<List<Plan>> GetPagedAndFilteredListAsync(int skipCount, int maxResultCount, string sorting, string filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        queryable = CreateFilteredQuery(queryable, filter)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Plan.Name) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await AsyncExecuter.ToListAsync(queryable, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetFilteredCountAsync(string filter, CancellationToken cancellationToken = default)
    {
        return await AsyncExecuter.CountAsync(
            CreateFilteredQuery(await GetQueryableAsync(), filter),
            GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Plan> CreateFilteredQuery(IQueryable<Plan> queryable, string filter)
    {
        if (filter.IsNullOrEmpty())
        {
            return queryable;
        }

        return queryable.Where(x => x.Name.ToLower().Contains(filter.ToLower()));
    }
}
