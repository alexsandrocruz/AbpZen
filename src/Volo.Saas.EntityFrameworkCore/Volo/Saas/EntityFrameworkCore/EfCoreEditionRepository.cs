using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;

namespace Volo.Saas.EntityFrameworkCore;

public class EfCoreEditionRepository : EfCoreRepository<ISaasDbContext, Edition, Guid>, IEditionRepository
{
    public EfCoreEditionRepository(IDbContextProvider<ISaasDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<Edition>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await GetListInternalAsync(sorting, maxResultCount, skipCount, filter, includeDetails, cancellationToken);
    }

    public virtual async Task<List<EditionWithTenantCount>> GetListWithTenantCountAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var editions = await GetListInternalAsync(sorting, maxResultCount, skipCount, filter, includeDetails, cancellationToken);

        var editionIds = editions.Select(x => x.Id).ToArray();
        var tenants = await (await GetDbContextAsync()).Set<Tenant>()
            .Where(x => x.EditionId.HasValue && editionIds.Contains(x.EditionId.Value))
            .GroupBy(x => x.EditionId)
            .Select(x => new
            {
                EditionId = x.Key,
                Count = x.Count()
            })
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        return editions.Select(edition => new EditionWithTenantCount
        {
            Edition = edition,
            TenantCount = tenants.FirstOrDefault(x => x.EditionId == edition.Id)?.Count ?? 0
        }).ToList();
    }

    protected virtual async Task<List<Edition>> GetListInternalAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.DisplayName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Edition.DisplayName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(
        string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.DisplayName.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> CheckNameExistAsync(string displayName, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(e => e.DisplayName == displayName)
            .AnyAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Edition> FindByDisplayNameAsync(string displayName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(t => t.DisplayName == displayName, GetCancellationToken(cancellationToken));
    }
}
