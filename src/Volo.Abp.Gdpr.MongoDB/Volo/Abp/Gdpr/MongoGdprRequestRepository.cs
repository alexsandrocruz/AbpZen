using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Gdpr;

public class MongoGdprRequestRepository : MongoDbRepository<GdprMongoDbContext, GdprRequest, Guid>, IGdprRequestRepository
{
    public MongoGdprRequestRepository(IMongoDbContextProvider<GdprMongoDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
        
    }

    public virtual async Task<long> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(r => r.UserId == userId)
            .As<IMongoQueryable<GdprRequest>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<GdprRequest>> GetListAsync(
        Guid userId, 
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,  
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(r => r.UserId == userId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(GdprRequest.CreationTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<GdprRequest>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<DateTime?> FindLatestRequestTimeOfUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreationTime)
            .Select(x => x.CreationTime)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}