using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.External;

namespace Volo.Abp.LanguageManagement.EntityFrameworkCore;

[ConnectionStringName(LanguageManagementDbProperties.ConnectionStringName)]
public interface ILanguageManagementDbContext : IEfCoreDbContext
{
    DbSet<Language> Languages { get; }

    DbSet<LanguageText> LanguageTexts { get; }
    
    DbSet<LocalizationResourceRecord> LocalizationResources { get; }
    
    DbSet<LocalizationTextRecord> LocalizationTexts { get; }
}
