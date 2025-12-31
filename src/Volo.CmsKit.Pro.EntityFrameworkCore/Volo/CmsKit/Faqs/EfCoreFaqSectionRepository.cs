using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Faqs;

public class EfCoreFaqSectionRepository : EfCoreRepository<ICmsKitProDbContext, FaqSection, Guid>, IFaqSectionRepository
{
    public EfCoreFaqSectionRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<FaqSectionWithQuestionCount>> GetListAsync(
        [CanBeNull] string filter = null,
        [CanBeNull] string sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var sectionQueryable = await GetQueryableAsync();
        var questionQueryable = dbContext.Set<FaqQuestion>().AsQueryable();

        var query = (from section in sectionQueryable
                     join question in questionQueryable on section.Id equals question.SectionId into questionsGroup
                     select new FaqSectionWithQuestionCount
                     {
                         Id = section.Id,
                         GroupName = section.GroupName,
                         Name = section.Name,
                         Order = section.Order,
                         QuestionCount = questionsGroup.Count(),
                         CreationTime = section.CreationTime
                     })
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Name.Contains(filter))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FaqSectionConst.DefaultSorting : sorting)
            .ThenBy((string.IsNullOrWhiteSpace(sorting) ? nameof(FaqSection.Name) : null) ?? string.Empty);


        return await query.PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task<long> GetCountAsync(
        [CanBeNull] string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Name.Contains(filter))
            .LongCountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<FaqSectionWithQuestions>> GetListSectionWithQuestionAsync(
        [CanBeNull] string groupName,
        [CanBeNull] string sectionName,
        CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var sectionQueryable = await GetQueryableAsync();
        var questionQueryable = dbContext.Set<FaqQuestion>().AsQueryable();
        
        var query =
            from section in sectionQueryable
            join question in questionQueryable on section.Id equals question.SectionId into sectionQuestions
            where (string.IsNullOrEmpty(groupName) || section.GroupName == groupName) &&
                  (string.IsNullOrEmpty(sectionName) || section.Name == sectionName)
            orderby section.Order
            select new FaqSectionWithQuestions
            {
                Section = section,
                Questions = sectionQuestions.OrderBy(q => q.Order).ToList()
            };
        
        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public virtual async Task<bool> AnyAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }
    
    public virtual async Task<bool> AnyAsync(
        [NotNull] string groupName,
        [NotNull] string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.GroupName == groupName && x.Name == name, GetCancellationToken(cancellationToken));
    }
}
