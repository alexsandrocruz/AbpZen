using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.MongoDB;

namespace Volo.Abp.LanguageManagement.MongoDB;

[ConnectionStringName(LanguageManagementDbProperties.ConnectionStringName)]
public class LanguageManagementMongoDbContext : AbpMongoDbContext, ILanguageManagementMongoDbContext
{
    public IMongoCollection<Language> Languages => Collection<Language>();

    public IMongoCollection<LanguageText> LanguageTexts => Collection<LanguageText>();
    
    public IMongoCollection<LocalizationResourceRecord> LocalizationResources  => Collection<LocalizationResourceRecord>();
    
    public IMongoCollection<LocalizationTextRecord> LocalizationTexts  => Collection<LocalizationTextRecord>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureLanguageManagement();
    }
}
