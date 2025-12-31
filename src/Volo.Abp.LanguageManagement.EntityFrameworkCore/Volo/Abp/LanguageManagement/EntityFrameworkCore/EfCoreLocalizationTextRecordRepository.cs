using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement.EntityFrameworkCore;

public class EfCoreLocalizationTextRecordRepository : EfCoreRepository<ILanguageManagementDbContext, LocalizationTextRecord, Guid>,
    ILocalizationTextRecordRepository
{
    public EfCoreLocalizationTextRecordRepository(IDbContextProvider<ILanguageManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public List<LocalizationTextRecord> GetList(
        string resourceName,
        string cultureName)
    {
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return DbSet
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
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(
            x => x.ResourceName == resourceName && x.CultureName == cultureName
        ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public LocalizationTextRecord? Find(
        string resourceName,
        string cultureName)
    {
#pragma warning disable 618
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return DbSet.FirstOrDefault(x => x.ResourceName == resourceName &&
                                             x.CultureName == cultureName);
        }
#pragma warning restore 618
    }

    public virtual async Task<LocalizationTextRecord?> FindAsync(
        string resourceName,
        string cultureName,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(x => x.ResourceName == resourceName && x.CultureName == cultureName)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}