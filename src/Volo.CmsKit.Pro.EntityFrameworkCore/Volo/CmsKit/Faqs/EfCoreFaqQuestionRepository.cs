using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using JetBrains.Annotations;

namespace Volo.CmsKit.Faqs;

public class EfCoreFaqQuestionRepository : EfCoreRepository<ICmsKitProDbContext, FaqQuestion, Guid>, IFaqQuestionRepository
{
    public EfCoreFaqQuestionRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<FaqQuestion>> GetListAsync(
        Guid sectionId,
        [CanBeNull] string filter = null,
        [CanBeNull] string sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.SectionId == sectionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Title.Contains(filter) || t.Text.Contains(filter))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FaqQuestionConst.DefaultSorting : sorting)
            .ThenBy((string.IsNullOrWhiteSpace(sorting) ? nameof(FaqQuestion.Title) : null) ?? string.Empty)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task<long> GetCountAsync(
        Guid sectionId,
        [CanBeNull] string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.SectionId == sectionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Title.Contains(filter) || t.Text.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task<bool> AnyAsync(
        Guid sectionId,
        [NotNull] string title,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(t => t.SectionId == sectionId && t.Title == title, GetCancellationToken(cancellationToken));
    }
}
