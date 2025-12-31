using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Volo.CmsKit.UrlShorting
{
    public class MongoShortenedUrlRepository : MongoDbRepository<ICmsKitProMongoDbContext, ShortenedUrl, Guid>, IShortenedUrlRepository
    {
        public MongoShortenedUrlRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<ShortenedUrl>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var token = GetCancellationToken(cancellationToken);

            var query = (await GetMongoQueryableAsync(token))
                .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Source.Contains(filter) || t.Target.Contains(filter));

            query = query.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(ShortenedUrl.CreationTime)} desc" : sorting);

            return await query.As<IMongoQueryable<ShortenedUrl>>()
                .PageBy<ShortenedUrl, IMongoQueryable<ShortenedUrl>>(skipCount, maxResultCount)
                .ToListAsync(token);
        }

        public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Source.Contains(filter) || t.Target.Contains(filter))
                .As<IMongoQueryable<ShortenedUrl>>()
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
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(x => x.IsRegex)
                .ToListAsync(cancellationToken);
        }
    }
}
