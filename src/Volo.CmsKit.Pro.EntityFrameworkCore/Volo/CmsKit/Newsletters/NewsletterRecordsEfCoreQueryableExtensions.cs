using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.CmsKit.Newsletters;

public static class NewsletterRecordsEfCoreQueryableExtensions
{
    public static IQueryable<NewsletterRecord> IncludeDetails(this IQueryable<NewsletterRecord> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Preferences);
    }
}
