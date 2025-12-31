using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class EfCoreTextTemplateDefinitionRecordRepository : EfCoreRepository<ITextTemplateManagementDbContext, TextTemplateDefinitionRecord, Guid>, ITextTemplateDefinitionRecordRepository
{
    public EfCoreTextTemplateDefinitionRecordRepository(IDbContextProvider<ITextTemplateManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<TextTemplateDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
