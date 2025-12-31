using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.LanguageManagement.MongoDB;

[DependsOn(
    typeof(LanguageManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class LanguageManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<LanguageManagementMongoDbContext>(options =>
        {
            options.AddRepository<Language, MongoLanguageRepository>();
            options.AddRepository<LanguageText, MongoLanguageTextRepository>();
            options.AddRepository<LocalizationResourceRecord, MongoLocalizationResourceRecordRepository>();
            options.AddRepository<LocalizationTextRecord, MongoLocalizationTextRecordRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
