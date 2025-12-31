using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public interface ITextTemplateDefinitionRecordRepository : IBasicRepository<TextTemplateDefinitionRecord, Guid>
{
    Task<TextTemplateDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
