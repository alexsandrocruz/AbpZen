using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.LanguageManagement.MongoDB;

public class MongoLanguageRepository : MongoDbRepository<ILanguageManagementMongoDbContext, Language, Guid>,
    ILanguageRepository
{
    public MongoLanguageRepository(IMongoDbContextProvider<ILanguageManagementMongoDbContext> dbContextProvider) :
        base(dbContextProvider)
    {
    }

    public virtual async Task<List<Language>> GetListByIsEnabledAsync(
        bool isEnabled,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(l => l.IsEnabled == isEnabled)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Language>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(filter != null,
                x => x.DisplayName.Contains(filter) ||
                     x.CultureName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(Language.CreationTime)} desc" : sorting)
            .As<IMongoQueryable<Language>>()
            .PageBy<Language, IMongoQueryable<Language>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }


    public virtual async Task<long> GetCountAsync(
        string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(filter != null,
                x => x.DisplayName.Contains(filter) ||
                     x.CultureName.Contains(filter))
            .As<IMongoQueryable<Language>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> AnyAsync(
        string cultureName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).AnyAsync(l => l.CultureName == cultureName, GetCancellationToken(cancellationToken));
    }
}
