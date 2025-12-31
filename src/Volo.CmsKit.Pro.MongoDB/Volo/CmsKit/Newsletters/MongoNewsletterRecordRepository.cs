using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Volo.CmsKit.Newsletters;

public class MongoNewsletterRecordRepository : MongoDbRepository<ICmsKitProMongoDbContext, NewsletterRecord, Guid>,
    INewsletterRecordRepository
{
    public MongoNewsletterRecordRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider) :
        base(dbContextProvider)
    {
    }

    public virtual async Task<List<NewsletterSummaryQueryResultItem>> GetListAsync(
        string preference = null,
        string source = null,
        string emailAddress = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var query = (await GetMongoQueryableAsync(token))
            .WhereIf(!preference.IsNullOrWhiteSpace(), q => q.Preferences.Any(x => x.Preference == preference))
            .WhereIf(!source.IsNullOrWhiteSpace(), q => q.Preferences.Any(x => x.Source.Contains(source)))
            .WhereIf(!emailAddress.IsNullOrWhiteSpace(), q => q.EmailAddress.Equals(emailAddress))
            .Select(t => new NewsletterSummaryQueryResultItem
            {
                Id = t.Id,
                EmailAddress = t.EmailAddress,
                CreationTime = t.CreationTime,
                Preferences = t.Preferences.Select(x => x.Preference).ToList()
            })
            .OrderByDescending(x => x.CreationTime);

        return await query.As<IMongoQueryable<NewsletterSummaryQueryResultItem>>()
            .PageBy<NewsletterSummaryQueryResultItem, IMongoQueryable<NewsletterSummaryQueryResultItem>>(skipCount, maxResultCount)
            .ToListAsync(token);
    }

    public virtual async Task<NewsletterRecord> FindByEmailAddressAsync(
        string emailAddress,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .Where(x => x.EmailAddress == emailAddress)
            .FirstOrDefaultAsync(token);
    }

    public virtual async Task<int> GetCountByFilterAsync(
        string preference = null,
        string source = null,
        string emailAddress = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var query = (await GetMongoQueryableAsync(token))
            .WhereIf(!preference.IsNullOrWhiteSpace(), q => q.Preferences.Any(x => x.Preference == preference))
            .WhereIf(!preference.IsNullOrWhiteSpace(), q => q.Preferences.Any(x => x.Source == source))
            .WhereIf(!emailAddress.IsNullOrWhiteSpace(), q => q.EmailAddress.Equals(emailAddress));

        return await query.As<IMongoQueryable<NewsletterRecord>>().CountAsync(token);
    }
}
