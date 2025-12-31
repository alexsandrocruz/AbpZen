using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Linq.Dynamic.Core;
using JetBrains.Annotations;

namespace Volo.CmsKit.Faqs;

public class MongoFaqQuestionRepository : MongoDbRepository<ICmsKitProMongoDbContext, FaqQuestion, Guid>, IFaqQuestionRepository
{
    public MongoFaqQuestionRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider) : base(dbContextProvider)
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
        var questionQueryable = await GetMongoQueryableAsync(cancellationToken);

        return await questionQueryable
            .Where(x => x.SectionId == sectionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Title.Contains(filter) || t.Text.Contains(filter))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FaqQuestionConst.DefaultSorting : sorting)
            .ThenBy((string.IsNullOrWhiteSpace(sorting) ? nameof(FaqQuestion.Title) : null) ?? string.Empty)
            .As<IMongoQueryable<FaqQuestion>>()
            .PageBy<FaqQuestion, IMongoQueryable<FaqQuestion>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<long> GetCountAsync(
       Guid sectionId,
       [CanBeNull] string filter = null,
       CancellationToken cancellationToken = default)
    {
        var questionQueryable = await GetMongoQueryableAsync(cancellationToken);

        return await questionQueryable
            .Where(x => x.SectionId == sectionId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Title.Contains(filter) || t.Text.Contains(filter))
            .As<IMongoQueryable<FaqQuestion>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task<bool> AnyAsync(
        Guid sectionId,
        [NotNull] string title,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .AnyAsync(t => t.SectionId == sectionId && t.Title == title, GetCancellationToken(cancellationToken));
    }
}
