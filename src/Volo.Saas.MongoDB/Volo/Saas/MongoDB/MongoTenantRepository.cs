using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Saas.Tenants;
using MongoDB.Driver.Core.Configuration;
using Volo.Saas.Editions;

namespace Volo.Saas.MongoDB;

public class MongoTenantRepository : MongoDbRepository<ISaasMongoDbContext, Tenant, Guid>, ITenantRepository
{
    public MongoTenantRepository(IMongoDbContextProvider<ISaasMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<Tenant> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(t => t.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Tenant> FindByNameAsync(
        string normalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(t => t.NormalizedName == normalizedName, GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use FindByIdAsync method.")]
    public virtual Tenant FindById(Guid id, bool includeDetails = true)
    {
        return GetMongoQueryable()
            .FirstOrDefault(t => t.Id == id);
    }

    [Obsolete("Use FindByNameAsync method.")]
    public virtual Tenant FindByName(string normalizedName, bool includeDetails = true)
    {
        return GetMongoQueryable()
            .FirstOrDefault(t => t.NormalizedName == normalizedName);
    }

    public virtual async Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        Guid? editionId = null,
        DateTime? expirationDateMin = null,
        DateTime? expirationDateMax = null,
        TenantActivationState? tenantActivationState = null,
        DateTime? activationEndDateMin = null,
        DateTime? activationEndDateMax = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetListQueryAsync(filter, editionId, expirationDateMin, expirationDateMax, tenantActivationState, activationEndDateMin, activationEndDateMax, cancellationToken))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Tenant.Name) : sorting)
            .As<IMongoQueryable<Tenant>>()
            .PageBy<Tenant, IMongoQueryable<Tenant>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Tenant>> GetListWithSeparateConnectionStringAsync(
        string connectionName = ConnectionStrings.DefaultConnectionStringName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(u => u.ConnectionStrings.Any(c => c.Name == connectionName && c.Value != null))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        Guid? editionId = null,
        DateTime? expirationDateMin = null,
        DateTime? expirationDateMax = null,
        TenantActivationState? tenantActivationState = null,
        DateTime? activationEndDateMin = null,
        DateTime? activationEndDateMax = null,
        CancellationToken cancellationToken = default
    )
    {
        return await (await GetListQueryAsync(filter, editionId, expirationDateMin, expirationDateMax, tenantActivationState, activationEndDateMin, activationEndDateMax, cancellationToken))
            .As<IMongoQueryable<Tenant>>()
            .CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task UpdateEditionsAsync(Guid sourceEditionId, Guid? targetEditionId = null, CancellationToken cancellationToken = default)
    {
        var tenants = await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).Where(x => x.EditionId == sourceEditionId).ToListAsync(GetCancellationToken(cancellationToken));
        foreach (var tenant in tenants)
        {
            tenant.EditionId = targetEditionId;
        }

        await UpdateManyAsync(tenants, cancellationToken: GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<Tenant>> GetListQueryAsync(
        string filter = null,
        Guid? editionId = null,
        DateTime? expirationDateMin = null,
        DateTime? expirationDateMax = null,
        TenantActivationState? tenantActivationState = null,
        DateTime? activationEndDateMin = null,
        DateTime? activationEndDateMax = null,
        CancellationToken cancellationToken = default)
    {
        return (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<Tenant, IMongoQueryable<Tenant>>(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter))
            .WhereIf(editionId.HasValue, tenant => tenant.EditionId == editionId.Value)
            .WhereIf(expirationDateMin.HasValue, tenant => tenant.EditionEndDateUtc >= expirationDateMin.Value)
            .WhereIf(expirationDateMax.HasValue, tenant => tenant.EditionEndDateUtc <= expirationDateMax.Value)
            .WhereIf(tenantActivationState.HasValue, tenant => tenant.ActivationState == tenantActivationState.Value)
            .WhereIf(activationEndDateMin.HasValue, tenant => tenant.ActivationEndDate >= activationEndDateMin)
            .WhereIf(activationEndDateMax.HasValue, tenant => tenant.ActivationEndDate <= activationEndDateMax);
    }
}
