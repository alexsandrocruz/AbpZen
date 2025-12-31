using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TextTemplateManagement.MongoDB;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class MongoDbTextTemplateDefinitionContentRecordRepository : MongoDbRepository<ITextTemplateManagementMongoDbContext, TextTemplateDefinitionContentRecord, Guid>, ITextTemplateDefinitionContentRecordRepository
{
    public MongoDbTextTemplateDefinitionContentRecordRepository(IMongoDbContextProvider<ITextTemplateManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<TextTemplateDefinitionContentRecord>> GetListByDefinitionIdAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.DefinitionId == definitionId)
            .As<IMongoQueryable<TextTemplateDefinitionContentRecord>>()
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
        var templateDefinitionRecordQueryable = await GetMongoQueryableAsync<TextTemplateDefinitionRecord>(cancellationToken);
        var definition = await templateDefinitionRecordQueryable.FirstAsync(x => x.Name == definitionName, cancellationToken: cancellationToken);

        if (definitionCultureName == null)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(x => x.DefinitionId == definition.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        var fileExtensions = new List<string>()
        {
            $"{definitionCultureName}.tpl",
            $"{definitionCultureName}.cshtml"
        };
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstAsync(x => x.DefinitionId == definition.Id && fileExtensions.Contains(x.FileName), GetCancellationToken(cancellationToken));
    }
}
