using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class EfCoreTextTemplateDefinitionContentRecordRepository : EfCoreRepository<ITextTemplateManagementDbContext, TextTemplateDefinitionContentRecord, Guid>, ITextTemplateDefinitionContentRecordRepository
{
    public EfCoreTextTemplateDefinitionContentRecordRepository(IDbContextProvider<ITextTemplateManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<TextTemplateDefinitionContentRecord>> GetListByDefinitionIdAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.DefinitionId == definitionId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteByDefinitionIdAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.DefinitionId == definitionId, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteByDefinitionIdAsync(Guid[] definitionIds, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => definitionIds.Contains(x.DefinitionId), cancellationToken: cancellationToken);
    }

    public virtual async Task<TextTemplateDefinitionContentRecord> FindByDefinitionNameAsync(string definitionName, string definitionCultureName = null, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var definition = await dbContext.Set<TextTemplateDefinitionRecord>().FirstAsync(x => x.Name == definitionName, cancellationToken: cancellationToken);

        if (definitionCultureName == null)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.DefinitionId == definition.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        var fileExtensions = new List<string>()
        {
            $"{definitionCultureName}.tpl",
            $"{definitionCultureName}.cshtml"
        };
        return await (await GetDbSetAsync()).FirstOrDefaultAsync(x => x.DefinitionId == definition.Id && fileExtensions.Contains(x.FileName), cancellationToken: cancellationToken);
    }
}
