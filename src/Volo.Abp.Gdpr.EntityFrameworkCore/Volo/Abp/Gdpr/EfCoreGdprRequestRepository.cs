using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Gdpr;

public class EfCoreGdprRequestRepository : EfCoreRepository<IGdprDbContext, GdprRequest, Guid>, IGdprRequestRepository
{
    public EfCoreGdprRequestRepository(IDbContextProvider<IGdprDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
        
    }

    public virtual async Task<long> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(r => r.UserId == userId)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<GdprRequest>> GetListAsync(
        Guid userId,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,  
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(r => r.UserId == userId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(GdprRequest.CreationTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<DateTime?> FindLatestRequestTimeOfUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreationTime)
            .Select(x => x.CreationTime)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
    
    public async override Task<IQueryable<GdprRequest>> WithDetailsAsync()
    {
        // Uses the extension method defined above
        return (await GetQueryableAsync()).Include(x=> x.Infos);
    }
}