using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Forms.EntityFrameworkCore;
using Volo.Forms.Forms;
using Volo.Forms.Questions;

namespace Volo.Forms;

public class EfCoreFormRepository : EfCoreRepository<IFormsDbContext, Form, Guid>, IFormRepository
{
    public EfCoreFormRepository(IDbContextProvider<IFormsDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<Form>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Title.Contains(filter) ||
                    u.Description.Contains(filter)
            ).OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Form.CreationTime) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<FormWithQuestions> GetWithQuestionsAsync(Guid id, bool includeChoices = true,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var questions = await dbContext.Questions
            .AsSingleQuery()
            .IncludeDetails(includeChoices)
            .Where(q => q.FormId == id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return new FormWithQuestions()
        {
            Form = await dbContext.Forms.FindAsync(id),
            Questions = questions.GroupBy(q => q.Id).Select(q => q.First())
                .ToList()
        };
    }

    public virtual async Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default
    )
    {
        return await (await GetQueryableAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Title.Contains(filter) ||
                     x.Description.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}
