using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Volo.CmsKit.Polls;
public static class EfCorePollQueryableExtensions
{
    public static IQueryable<Poll> IncludeDetails(this IQueryable<Poll> queryable, bool include = true)
    {
        return !include
                ? queryable
                : queryable
                    .Include(q => q.PollOptions);
    }
}
