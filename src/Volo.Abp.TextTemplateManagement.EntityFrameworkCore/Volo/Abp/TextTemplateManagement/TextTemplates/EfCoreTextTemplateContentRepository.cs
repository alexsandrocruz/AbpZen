using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class EfCoreTextTemplateContentRepository : EfCoreRepository<ITextTemplateManagementDbContext, TextTemplateContent, Guid>,
    ITextTemplateContentRepository
{
    public EfCoreTextTemplateContentRepository(IDbContextProvider<ITextTemplateManagementDbContext> dbContextProvider) : base(dbContextProvider)
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
