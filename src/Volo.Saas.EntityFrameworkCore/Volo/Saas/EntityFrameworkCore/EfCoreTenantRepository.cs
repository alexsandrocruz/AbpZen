using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Saas.Tenants;

namespace Volo.Saas.EntityFrameworkCore;

public class EfCoreTenantRepository : EfCoreRepository<ISaasDbContext, Tenant, Guid>, ITenantRepository
{
    public EfCoreTenantRepository(IDbContextProvider<ISaasDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<Tenant> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Tenant> FindByNameAsync(
        string normalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(t => t.NormalizedName == normalizedName, GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use FindByIdAsync method.")]
    public Tenant FindById(Guid id, bool includeDetails = true)
    {
        return DbSet
            .IncludeDetails(includeDetails)
            .FirstOrDefault(t => t.Id == id);
    }

    [Obsolete("Use FindByNameAsync method.")]
    public Tenant FindByName(string normalizedName, bool includeDetails = true)
    {
        return DbSet
            .IncludeDetails(includeDetails)
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
            .IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Tenant.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Tenant>> GetListWithSeparateConnectionStringAsync(
        string connectionName = ConnectionStrings.DefaultConnectionStringName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
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
            .CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task UpdateEditionsAsync(Guid sourceEditionId, Guid? targetEditionId = null, CancellationToken cancellationToken = default)
    {
        await (await GetDbSetAsync()).Where(x => x.EditionId == sourceEditionId).ExecuteUpdateAsync(
            t => t.SetProperty(e => e.EditionId, targetEditionId), GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<Tenant> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public override async Task<IQueryable<Tenant>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
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
        return (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),u => u.Name.Contains(filter))
            .WhereIf(editionId.HasValue, tenant => tenant.EditionId == editionId.Value)
            .WhereIf(expirationDateMin.HasValue, tenant => tenant.EditionEndDateUtc >= expirationDateMin)
            .WhereIf(expirationDateMax.HasValue, tenant => tenant.EditionEndDateUtc <= expirationDateMax)
            .WhereIf(tenantActivationState.HasValue, tenant => tenant.ActivationState == tenantActivationState)
            .WhereIf(activationEndDateMin.HasValue, tenant => tenant.ActivationEndDate >= activationEndDateMin)
            .WhereIf(activationEndDateMax.HasValue, tenant => tenant.ActivationEndDate <= activationEndDateMax);
    }
}
