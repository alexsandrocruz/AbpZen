using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement.MongoDB;

public class MongoLocalizationResourceRecordRepository :
    MongoDbRepository<ILanguageManagementMongoDbContext, LocalizationResourceRecord, Guid>,
    ILocalizationResourceRecordRepository
{
    public MongoLocalizationResourceRecordRepository(IMongoDbContextProvider<ILanguageManagementMongoDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public LocalizationResourceRecord? Find(string name)
    {
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return GetMongoQueryable()
                .FirstOrDefault(x => x.Name == name);
        }
#pragma warning restore 618
    }

    public virtual async Task<LocalizationResourceRecord?> FindAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await FindAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }
}