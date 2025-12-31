using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public interface ITextTemplateDefinitionContentRecordRepository : IBasicRepository<TextTemplateDefinitionContentRecord, Guid>
{
    Task<List<TextTemplateDefinitionContentRecord>> GetListByDefinitionIdAsync(Guid definitionId, CancellationToken cancellationToken = default);

    Task DeleteByDefinitionIdAsync(Guid definitionId, CancellationToken cancellationToken = default);

    Task DeleteByDefinitionIdAsync(Guid[] definitionIds, CancellationToken cancellationToken = default);

    Task<TextTemplateDefinitionContentRecord> FindByDefinitionNameAsync(string definitionName, string definitionCultureName = null, CancellationToken cancellationToken = default);
}
