using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Gdpr;

public interface IGdprRequestRepository : IBasicRepository<GdprRequest, Guid>
{
    Task<long> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<List<GdprRequest>> GetListAsync(
        Guid userId, 
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0, 
        CancellationToken cancellationToken = default);
    
    Task<DateTime?> FindLatestRequestTimeOfUserAsync(Guid userId, CancellationToken cancellationToken = default);
}