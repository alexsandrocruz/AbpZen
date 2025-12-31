using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Forms.Questions;

public interface IQuestionRepository : IBasicRepository<QuestionBase, Guid>
{
    Task<List<QuestionBase>> GetListByFormIdAsync(
        Guid formId,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<QuestionWithChoices> GetWithChoices(Guid id, CancellationToken cancellationToken = default);

    Task<List<QuestionWithAnswers>> GetListWithAnswersByFormResponseId(Guid formResponseId,
        CancellationToken cancellationToken = default);

    Task<List<QuestionWithAnswers>> GetListWithAnswersByFormId(Guid formId,
        CancellationToken cancellationToken = default);

    Task ClearQuestionChoicesAsync(Guid itemId, CancellationToken cancellationToken = default);
}
