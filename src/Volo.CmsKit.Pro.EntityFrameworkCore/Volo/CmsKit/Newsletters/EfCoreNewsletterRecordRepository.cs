using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Newsletters;

public class EfCoreNewsletterRecordRepository : EfCoreRepository<ICmsKitProDbContext, NewsletterRecord, Guid>,
    INewsletterRecordRepository
{
    public EfCoreNewsletterRecordRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider)
        : base(dbContextProvider)
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
        var query = (await GetDbSetAsync())
            .WhereIf(!preference.IsNullOrWhiteSpace(), t => t.Preferences.Any(x => x.Preference == preference))
            .WhereIf(!source.IsNullOrWhiteSpace(), t => t.Preferences.Any(x => x.Source.Contains(source)))
            .WhereIf(!emailAddress.IsNullOrWhiteSpace(), t => t.EmailAddress.Equals(emailAddress))
            .Include(x => x.Preferences)
            .Select(t => new NewsletterSummaryQueryResultItem
            {
                Id = t.Id,
                EmailAddress = t.EmailAddress,
                CreationTime = t.CreationTime,
                Preferences = t.Preferences.Select(x => x.Preference).ToList()
            })
            .OrderByDescending(x => x.CreationTime);

        return await query.PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<NewsletterRecord> FindByEmailAddressAsync(
        string emailAddress,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.EmailAddress == emailAddress)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountByFilterAsync(
        string preference = null,
        string source = null,
        string emailAddress = null,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
            .WhereIf(!preference.IsNullOrWhiteSpace(), t => t.Preferences.Any(x => x.Preference == preference))
            .WhereIf(!source.IsNullOrWhiteSpace(), t => t.Preferences.Any(x => x.Source.Contains(source)))
            .WhereIf(!emailAddress.IsNullOrWhiteSpace(), t => t.EmailAddress.Equals(emailAddress));

        return await query.CountAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<IQueryable<NewsletterRecord>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
