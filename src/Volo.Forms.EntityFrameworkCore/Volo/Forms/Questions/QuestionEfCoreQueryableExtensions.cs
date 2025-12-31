using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.Forms.Questions;

public static class QuestionEfCoreQueryableExtensions
{
    public static IQueryable<QuestionBase> IncludeDetails(this IQueryable<QuestionBase> queryable,
        bool include = true)
    {
        //Bug: Magic string and EfCore bug:https://github.com/dotnet/efcore/issues/22016
        //Bug: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/breaking-changes#some-queries-with-correlated-collection-that-also-use-distinct-or-groupby-are-no-longer-supported
        return !include ? queryable : queryable.Include("Choices");
    }
}
