using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;

namespace Volo.Saas.MongoDB;

public class MongoEditionRepository : MongoDbRepository<ISaasMongoDbContext, Edition, Guid>, IEditionRepository
{
    public MongoEditionRepository(IMongoDbContextProvider<ISaasMongoDbContext> dbContextProvider)
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
        var tenants = await (await GetMongoQueryableAsync<Tenant>(cancellationToken))
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
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.DisplayName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Edition.DisplayName) : sorting)
            .As<IMongoQueryable<Edition>>()
            .PageBy<Edition, IMongoQueryable<Edition>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(
        string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.DisplayName.Contains(filter))
            .As<IMongoQueryable<Edition>>()
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> CheckNameExistAsync(string displayName, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(e => e.DisplayName == displayName)
            .AnyAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Edition> FindByDisplayNameAsync(string displayName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(t => t.DisplayName == displayName, GetCancellationToken(cancellationToken));
    }
}
