using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement.MongoDB;

public class MongoLocalizationTextRecordRepository :
    MongoDbRepository<ILanguageManagementMongoDbContext, LocalizationTextRecord, Guid>,
    ILocalizationTextRecordRepository
{
    public MongoLocalizationTextRecordRepository(IMongoDbContextProvider<ILanguageManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public List<LocalizationTextRecord> GetList(string resourceName, string cultureName)
    {
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return GetMongoQueryable()
                .Where(l => l.ResourceName == resourceName && l.CultureName == cultureName)
                .ToList();
        }
#pragma warning restore 618
    }

    public virtual async Task<List<LocalizationTextRecord>> GetListAsync(
        string resourceName,
        string cultureName,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));
        return await queryable
            .Where(x => x.ResourceName == resourceName && x.CultureName == cultureName)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public LocalizationTextRecord? Find(string resourceName, string cultureName)
    {
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return GetMongoQueryable()
                .Where(x => x.ResourceName == resourceName &&
                            x.CultureName == cultureName)
                .FirstOrDefault();
        }
#pragma warning restore 618
    }

    public virtual async Task<LocalizationTextRecord?> FindAsync(string resourceName, string cultureName, CancellationToken cancellationToken = default)
    {
        var queryable = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));
        return await queryable
            .Where(x => x.ResourceName == resourceName && x.CultureName == cultureName)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}