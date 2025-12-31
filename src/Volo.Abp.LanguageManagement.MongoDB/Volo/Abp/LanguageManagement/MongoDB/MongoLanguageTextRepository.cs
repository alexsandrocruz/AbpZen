using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement.MongoDB;

public class MongoLanguageTextRepository : MongoDbRepository<ILanguageManagementMongoDbContext, LanguageText, Guid>, ILanguageTextRepository
{
    public MongoLanguageTextRepository(IMongoDbContextProvider<ILanguageManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {

    }

    public virtual List<LanguageText> GetList(string resourceName, string cultureName)
    {
        //GetList should be sync because DynamicResourceLocalizer must use it in a sync way!
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return GetMongoQueryable()
                .Where(l => l.ResourceName == resourceName && l.CultureName == cultureName)
                .ToList();
        }
#pragma warning restore 618
    }

    public virtual async Task<List<LanguageText>> GetListAsync(string resourceName, string cultureName, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(l => l.ResourceName == resourceName && l.CultureName == cultureName)
            .ToListAsync(cancellationToken);
    }
}
