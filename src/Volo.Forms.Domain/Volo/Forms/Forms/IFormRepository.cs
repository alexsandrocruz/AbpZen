using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Forms.Forms;

public interface IFormRepository : IBasicRepository<Form, Guid>
{
    Task<List<Form>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task<FormWithQuestions> GetWithQuestionsAsync(
        Guid id,
        bool includeChoices = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default
    );
}
