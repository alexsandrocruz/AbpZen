using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Payment.EntityFrameworkCore;

namespace Volo.Payment.Plans;

public class EfCorePlanRepository : EfCoreRepository<IPaymentDbContext, Plan, Guid>, IPlanRepository
{
    public EfCorePlanRepository(IDbContextProvider<IPaymentDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<GatewayPlan> GetGatewayPlanAsync(Guid planId, string gateway)
    {
        var context = await GetDbContextAsync();

        return await context.Set<GatewayPlan>().FirstOrDefaultAsync(x => x.PlanId == planId && x.Gateway == gateway)
            ?? throw new EntityNotFoundException(typeof(GatewayPlan));
    }

    public virtual async Task InsertGatewayPlanAsync(GatewayPlan gatewayPlan)
    {
        var context = await GetDbContextAsync();

        context.Entry(gatewayPlan).State = EntityState.Added;
    }

    public virtual async Task DeleteGatewayPlanAsync(Guid planId, string gateway)
    {
        var gatewayPlan = await GetGatewayPlanAsync(planId, gateway);

        var context = await GetDbContextAsync();

        context.Entry(gatewayPlan).State = EntityState.Deleted;
    }

    public virtual async Task UpdateGatewayPlanAsync(GatewayPlan gatewayPlan)
    {
        var context = await GetDbContextAsync();

        context.Entry(gatewayPlan).State = EntityState.Modified;
    }

    public override async Task<IQueryable<Plan>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync()).Include(i => i.GatewayPlans);
    }

    public virtual async Task<List<GatewayPlan>> GetGatewayPlanPagedListAsync(Guid planId, int skipCount, int maxResultCount, string sorting, string filter = null)
    {
        var context = await GetDbContextAsync();

        var queryable = context.GatewayPlans
            .Where(x => x.PlanId == planId)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(GatewayPlan.Gateway) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        if (!filter.IsNullOrEmpty())
        {
            queryable = queryable.Where(x => x.Gateway.ToLower().Contains(filter) || x.ExternalId.ToLower().Contains(filter));
        }

        return await queryable.ToListAsync(GetCancellationToken());
    }

    public virtual async Task<int> GetGatewayPlanCountAsync(Guid planId, string filter = null)
    {
        var context = await GetDbContextAsync();
        var queryable = context.GatewayPlans
            .Where(x => x.PlanId == planId)
            .WhereIf(!filter.IsNullOrEmpty(), x => x.Gateway.ToLower().Contains(filter) || x.ExternalId.ToLower().Contains(filter));

        return await queryable.CountAsync();
    }

    public virtual async Task<List<Plan>> GetManyAsync(Guid[] ids)
    {
        return await (await GetQueryableAsync()).Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public virtual async Task<List<Plan>> GetPagedAndFilteredListAsync(int skipCount, int maxResultCount, string sorting, string filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var queryable = (includeDetails ? await WithDetailsAsync() : await GetQueryableAsync());
        
        queryable = CreateFilteredQuery(queryable, filter)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Plan.Name) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetFilteredCountAsync(string filter, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await CreateFilteredQuery(queryable, filter).CountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Plan> CreateFilteredQuery(IQueryable<Plan> queryable, string filter)
    {
        return queryable.WhereIf(!filter.IsNullOrEmpty(), x => x.Name.ToLower().Contains(filter.ToLower()));
    }
}
