using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TextTemplateManagement.MongoDB;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class MongoDbTextTemplateDefinitionRecordRepository : MongoDbRepository<ITextTemplateManagementMongoDbContext, TextTemplateDefinitionRecord, Guid>, ITextTemplateDefinitionRecordRepository
{
    public MongoDbTextTemplateDefinitionRecordRepository(
        IMongoDbContextProvider<ITextTemplateManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<TextTemplateDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await FindAsync(x => x.Name == name, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
