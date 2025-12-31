using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TextTemplateManagement.MongoDB;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class MongoDbTextTemplateContentRepository :
    MongoDbRepository<ITextTemplateManagementMongoDbContext, TextTemplateContent, Guid>,
    ITextTemplateContentRepository
{
    public MongoDbTextTemplateContentRepository(IMongoDbContextProvider<ITextTemplateManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<TextTemplateContent> GetAsync(
        string name,
        string cultureName = null,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync(x => x.Name == name && x.CultureName == cultureName, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<TextTemplateContent> FindAsync(
        string name,
        string cultureName = null,
        CancellationToken cancellationToken = default)
    {
        return await FindAsync(x => x.Name == name && x.CultureName == cultureName, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
