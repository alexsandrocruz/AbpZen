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

public class MongoFaqSectionRepository : MongoDbRepository<ICmsKitProMongoDbContext, FaqSection, Guid>, IFaqSectionRepository
{
    public MongoFaqSectionRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<FaqSectionWithQuestionCount>> GetListAsync(
         [CanBeNull] string filter = null,
         [CanBeNull] string sorting = null,
         int skipCount = 0,
         int maxResultCount = int.MaxValue,
         CancellationToken cancellationToken = default)
    {
        var sectionQueryable = await GetMongoQueryableAsync(cancellationToken);
        var questionQueryable = await GetMongoQueryableAsync<FaqQuestion>(cancellationToken);
        
        var sections = await sectionQueryable
            .WhereIf(!string.IsNullOrWhiteSpace(filter), t => t.Name.Contains(filter))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FaqSectionConst.DefaultSorting : sorting)
            .ThenBy((string.IsNullOrWhiteSpace(sorting) ? nameof(FaqSection.Name) : null) ?? string.Empty)
            .Select(section => new FaqSectionWithQuestionCount
            {
                Id = section.Id,
                GroupName = section.GroupName,
                Name = section.Name,
                Order = section.Order,
                QuestionCount = 0,
                CreationTime = section.CreationTime
            })
            .As<IMongoQueryable<FaqSectionWithQuestionCount>>()
            .PageBy<FaqSectionWithQuestionCount, IMongoQueryable<FaqSectionWithQuestionCount>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
        
        var sectionIds = sections.Select(s => s.Id).ToList();
        
        var questionCounts = await questionQueryable
            .Where(q => sectionIds.Contains(q.SectionId))
            .GroupBy(q => q.SectionId)
            .Select(g => new { SectionId = g.Key, Count = g.Count() })
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var section in sections)
        {
            section.QuestionCount = questionCounts.FirstOrDefault(q => q.SectionId == section.Id)?.Count ?? 0;
        }
        
        return sections;
    }

    public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Name.Contains(filter))
            .As<IMongoQueryable<FaqSection>>()
            .LongCountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }


    public virtual async Task<List<FaqSectionWithQuestions>> GetListSectionWithQuestionAsync(
         [CanBeNull] string groupName,
         [CanBeNull] string sectionName,
         CancellationToken cancellationToken)
    {
        var sectionQueryable = await GetMongoQueryableAsync(cancellationToken);
        var questionQueryable = await GetMongoQueryableAsync<FaqQuestion>(cancellationToken);
       
        var sectionWithQuestions = await sectionQueryable
            .WhereIf(!string.IsNullOrWhiteSpace(groupName), t => t.GroupName.Contains(groupName))
            .WhereIf(!string.IsNullOrWhiteSpace(sectionName), t => t.Name.Contains(sectionName))
            .OrderBy(x => x.Order)
            .Select(section => new FaqSectionWithQuestions
            {
                Section = section,
                Questions = new List<FaqQuestion>()
            }) 
            .As<IMongoQueryable<FaqSectionWithQuestions>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
        
        
        var sectionIds = sectionWithQuestions.Select(s => s.Section.Id).ToList();
        
        var questions = await questionQueryable
            .Where(q => sectionIds.Contains(q.SectionId))
            .OrderBy(q => q.Order)
            .GroupBy(q => q.SectionId)
            .Select(g => new { SectionId = g.Key, Questions = g.ToList() })
            .ToListAsync(GetCancellationToken(cancellationToken)); 
        
        foreach (var section in sectionWithQuestions)
        {
            section.Questions = questions.FirstOrDefault(q => q.SectionId == section.Section.Id)?.Questions ?? new List<FaqQuestion>();
        }
        
        return sectionWithQuestions;
    }

    public virtual async Task<bool> AnyAsync(
         Guid id,
         CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .AnyAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }
    
    public async Task<bool> AnyAsync([NotNull] string groupName, [NotNull] string name, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .AnyAsync(x => x.GroupName == groupName && x.Name == name, GetCancellationToken(cancellationToken));
    }
}
