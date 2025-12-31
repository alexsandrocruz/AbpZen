using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Forms.Questions;
using Volo.Forms.MongoDB;
using Volo.Forms.Questions.ChoosableItems;
using Volo.Forms.Responses;

namespace Volo.Forms;

public class MongoQuestionRepository : MongoDbRepository<IFormsMongoDbContext, QuestionBase, Guid>,
    IQuestionRepository
{
    public MongoQuestionRepository(IMongoDbContextProvider<IFormsMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<QuestionBase>> GetListByFormIdAsync(
        Guid formId,
        string sorting = null,
        int maxResultCount = Int32.MaxValue,
        int skipCount = 0,
        string filter = null, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await Queryable.Where(await GetMongoQueryableAsync(cancellationToken), q => q.FormId == formId)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Title.Contains(filter) ||
                    u.Description.Contains(filter)
            )
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(QuestionBase.Index) : sorting)
            .As<IMongoQueryable<QuestionBase>>()
            .PageBy<QuestionBase, IMongoQueryable<QuestionBase>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<QuestionWithChoices> GetWithChoices(Guid id,
        CancellationToken cancellationToken = default)
    {
        var item = await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(q => q.Id == id, GetCancellationToken(cancellationToken));
        return new QuestionWithChoices()
        {
            Question = item,
            Choices = (item as IChoosable)?.GetChoices().ToList()
        };
    }

    public virtual async Task<List<QuestionWithAnswers>> GetListWithAnswersByFormResponseId(Guid formResponseId,
        CancellationToken cancellationToken = default)
    {
        var formResponse = await (await GetMongoQueryableAsync<FormResponse>(cancellationToken))
            .FirstAsync(q => q.Id == formResponseId, GetCancellationToken(cancellationToken));

        var items = new List<QuestionWithAnswers>();
        var questions = await (await GetMongoQueryableAsync<QuestionBase>(cancellationToken))
            .Where(q => q.FormId == formResponse.FormId)
            .OrderBy(q => q.Index)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var questionIds = questions.Select(q => q.Id);

        var answers = formResponse.Answers
            .Where(a => questionIds.Contains(a.QuestionId) && a.FormResponseId == formResponseId).ToList();

        foreach (var item in questions)
        {
            items.Add(new QuestionWithAnswers()
            {
                Question = item,
                Choices = (item as IChoosable)?.GetChoices().ToList(),
                Answers = answers.Where(q => q.QuestionId == item.Id).ToList()
            });
        }

        return items;
    }

    public virtual async Task<List<QuestionWithAnswers>> GetListWithAnswersByFormId(Guid formId,
        CancellationToken cancellationToken = default)
    {
        var questions = await (await GetMongoQueryableAsync<QuestionBase>(cancellationToken))
            .Where(q => q.FormId == formId)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        var answers = await (await GetMongoQueryableAsync<FormResponse>(cancellationToken))
            .Where(q => q.FormId == formId)
            .SelectMany(t => t.Answers)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return questions.OrderBy(q => q.Index)
            .Select(question => new QuestionWithAnswers()
            {
                Question = question,
                Answers = answers.Where(q => q.QuestionId == question.Id).ToList(),
                Choices = (question as IChoosable)?.GetChoices().ToList()
            }).ToList();
    }

    public virtual async Task ClearQuestionChoicesAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(GetCancellationToken(cancellationToken));
        var item = await (await GetMongoQueryableAsync<QuestionBase>(cancellationToken))
            .FirstAsync(q => q.Id == itemId, GetCancellationToken(cancellationToken));
        (item as IChoosable)?.ClearChoices();

        await dbContext.Questions.ReplaceOneAsync(u => u.Id == itemId, item,
            cancellationToken: GetCancellationToken(cancellationToken));
    }
}
