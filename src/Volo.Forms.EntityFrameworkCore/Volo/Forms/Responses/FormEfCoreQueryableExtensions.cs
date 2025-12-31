using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.Forms.Responses;

public static class EfCoreResponsesQueryableExtensions
{
    public static IQueryable<FormResponse> IncludeDetails(this IQueryable<FormResponse> queryable,
        bool include = true)
    {
        return !include ? queryable : queryable.Include(a => a.Answers);
    }
}
