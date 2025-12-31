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
using Volo.Forms.Forms;
using Volo.Forms.MongoDB;
using Volo.Forms.Questions;

namespace Volo.Forms;

public class MongoFormRepository : MongoDbRepository<IFormsMongoDbContext, Form, Guid>, IFormRepository
{
    public MongoFormRepository(IMongoDbContextProvider<IFormsMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<Form>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Title.Contains(filter) ||
                    u.Description.Contains(filter)
            )
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Form.Title) : sorting)
            .As<IMongoQueryable<Form>>()
            .PageBy<Form, IMongoQueryable<Form>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<FormWithQuestions> GetWithQuestionsAsync(
        Guid id,
        bool includeChoices = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return new FormWithQuestions
        {
            Form = await (await GetMongoQueryableAsync<Form>(cancellationToken)).FirstOrDefaultAsync(q => q.Id == id, cancellationToken),
            Questions = await (await GetMongoQueryableAsync<QuestionBase>(cancellationToken)).Where(q => q.FormId == id).ToListAsync(cancellationToken)
        };
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Title.Contains(filter) ||
                     x.Description.Contains(filter))
            .As<IMongoQueryable<Form>>()
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    // public virtual async Task<FormWithAnswers> GetQuestionsWithAnswersAsync(
    //     Guid id,
    //     CancellationToken cancellationToken = default)
    // {
    //
    //     var form = await GetMongoQueryable().FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    //     var items = await DbContext.Questions.AsQueryable().Where(q => q.FormId == id).ToListAsync(cancellationToken);
    //     var itemIds = items.Select(q => q.Id);
    //     //TODO: This needs to be changed-> Answers are under form response, not in different document
    //     var answers = await DbContext.Answers.AsQueryable()
    //         .Where(a => itemIds.Contains(a.QuestionId)).ToListAsync(GetCancellationToken(cancellationToken));
    //     return new FormWithAnswers()
    //     {
    //         Form = form,
    //         Answers = answers,
    //         Items = items
    //     };
    // }
}
