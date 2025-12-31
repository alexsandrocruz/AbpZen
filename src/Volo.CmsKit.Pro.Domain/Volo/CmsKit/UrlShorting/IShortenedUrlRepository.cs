using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.UrlShorting;

public interface IShortenedUrlRepository: IBasicRepository<ShortenedUrl, Guid>
{
    Task<List<ShortenedUrl>> GetListAsync(
        string filter = null,
        string Sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);

    Task<ShortenedUrl> FindBySourceUrlAsync(
        string sourceUrl,
        CancellationToken cancellationToken = default);
    
    Task<List<ShortenedUrl>> GetAllRegexAsync(
        CancellationToken cancellationToken = default);
}
