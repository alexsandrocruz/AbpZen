using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Volo.CmsKit.UrlShorting
{
    public class EfCoreShortenedUrlRepository : EfCoreRepository<ICmsKitProDbContext, ShortenedUrl, Guid>, IShortenedUrlRepository
    {
        public EfCoreShortenedUrlRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<ShortenedUrl>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = 2147483647,
            CancellationToken cancellationToken = default)
        {
            var query = (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Source.Contains(filter) || t.Target.Contains(filter));

            query = query.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(ShortenedUrl.CreationTime)} desc" : sorting);

            return await query.PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Source.Contains(filter) || t.Target.Contains(filter))
                .LongCountAsync(cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ShortenedUrl> FindBySourceUrlAsync(
            string sourceUrl,
            CancellationToken cancellationToken = default)
        {
            return await base.FindAsync(x => x.Source == sourceUrl, cancellationToken: cancellationToken);
        }

        public async Task<List<ShortenedUrl>> GetAllRegexAsync(CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.IsRegex)
                .ToListAsync(cancellationToken);
        }
    }
}
