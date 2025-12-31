using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Forms.EntityFrameworkCore;

namespace Volo.Forms.Questions;

public class EfCoreQuestionRepository : EfCoreRepository<IFormsDbContext, QuestionBase, Guid>, IQuestionRepository
{
    public EfCoreQuestionRepository(IDbContextProvider<IFormsDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<QuestionBase>> GetListByFormIdAsync(
        Guid formId,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetQueryableAsync())
            .AsSingleQuery()
            .Where(q => q.FormId == formId)
            .IncludeDetails(includeDetails)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Title.Contains(filter) ||
                    u.Description.Contains(filter)
            ).OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(QuestionBase.Index) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return result.GroupBy(q => q.Id).Select(q => q.First())
                .ToList(); // Client side grouping because of EfCore bug:https://github.com/dotnet/efcore/issues/22016
    }

    public virtual async Task<QuestionWithChoices> GetWithChoices(Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return new QuestionWithChoices()
        {
            Question = await dbContext.Questions.FindAsync(new object[] { id },
                GetCancellationToken(cancellationToken)),
            Choices = await dbContext.Choices.Where(q => q.ChoosableQuestionId == id)
                .ToListAsync(GetCancellationToken(cancellationToken))
        };
    }

    public virtual async Task<List<QuestionWithAnswers>> GetListWithAnswersByFormResponseId(Guid formResponseId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var formResponse = await dbContext.FormResponses
            .AsSingleQuery()
            .Include(q => q.Answers)
            .FirstAsync(q => q.Id == formResponseId, cancellationToken: GetCancellationToken(cancellationToken));

        var questions = await dbContext.Questions
            .IncludeDetails()
            .Where(q => q.FormId == formResponse.FormId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var sortedQuestions = questions
            .GroupBy(q => q.Id)
            .Select(q => q.First())
            .ToList();

        var questionIds = questions.Select(q => q.Id).ToList();
        var answers = formResponse.Answers
            .Where(a => questionIds.Contains(a.QuestionId) && a.FormResponseId == formResponseId).ToList();

        List<QuestionWithAnswers> items = new List<QuestionWithAnswers>();
        foreach (var item in sortedQuestions.OrderBy(q => q.Index))
        {
            items.Add(new QuestionWithAnswers()
            {
                Question = item,
                Choices = await dbContext.Choices
                    .Where(q => q.ChoosableQuestionId == item.Id)
                    .ToListAsync(GetCancellationToken(cancellationToken)),
                Answers = answers.Where(q => q.QuestionId == item.Id).ToList()
            });
        }

        return items;
    }

    public virtual async Task<List<QuestionWithAnswers>> GetListWithAnswersByFormId(Guid formId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var questions = await dbContext.Questions.Where(q => q.FormId == formId)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
        var questionIds = questions.Select(q => q.Id);

        var answers = await dbContext.Answers.Where(q => questionIds.Contains(q.QuestionId))
            .ToListAsync(GetCancellationToken(cancellationToken));

        var choices = await dbContext.Choices.Where(q => questionIds.Contains(q.ChoosableQuestionId))
            .ToListAsync(GetCancellationToken(cancellationToken));

        return questions
            .OrderBy(q => q.Index)
            .Select(question => new QuestionWithAnswers()
            {
                Question = question,
                Answers = answers.Where(q => q.QuestionId == question.Id).ToList(),
                Choices = choices.Where(q => q.ChoosableQuestionId == question.Id).ToList()
            }).ToList();
    }

    public override async Task<QuestionBase> GetAsync(
        Guid id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var dbSet = await GetQueryableAsync();
            var query = (await dbSet
                    .AsSingleQuery()
                    .IncludeDetails(includeDetails)
                    .Select(p => p)
                    .ToListAsync(GetCancellationToken(cancellationToken)))
                .Distinct()
                .ToList();

            return query.FirstOrDefault(q => q.Id == id);

            //Ef Core bug
            return await dbSet
                .AsSingleQuery()
                .IncludeDetails(includeDetails) // Todo: will be removed -> efCore bug
                .Distinct()
                .FirstOrDefaultAsync(q => q.Id == id, GetCancellationToken(cancellationToken));
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(
                $"EfCoreBug -> multiple same id item found because of include not working properly: {e}");
            throw;
        }
    }

    public virtual async Task ClearQuestionChoicesAsync(
        Guid itemId,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = await GetDbContextAsync();
        var choices = await dbContext.Choices.Where(q => q.ChoosableQuestionId == itemId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        dbContext.Choices.RemoveRange(choices);
    }

    public override async Task<IQueryable<QuestionBase>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
