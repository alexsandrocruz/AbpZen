using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BackgroundJobs.MongoDB;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.LanguageManagement.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Abp.TextTemplateManagement.MongoDB;
using Volo.Saas.MongoDB;
using Volo.Abp.BlobStoring.Database.MongoDB;
using Volo.Abp.Uow;
using Volo.Chat.MongoDB;
using Volo.CmsKit.MongoDB;
using Volo.FileManagement.MongoDB;
using Volo.Forms.MongoDB;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict.MongoDB;

namespace LeptonXDemoApp.MongoDB
{
    [DependsOn(
        typeof(LeptonXDemoAppDomainModule),
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpIdentityProMongoDbModule),
        typeof(AbpOpenIddictMongoDbModule),
        typeof(AbpBackgroundJobsMongoDbModule),
        typeof(AbpAuditLoggingMongoDbModule),
        typeof(AbpFeatureManagementMongoDbModule),
        typeof(LanguageManagementMongoDbModule),
        typeof(SaasMongoDbModule),
        typeof(TextTemplateManagementMongoDbModule),
        typeof(BlobStoringDatabaseMongoDbModule)
    )]
    [DependsOn(typeof(ChatMongoDbModule))]
    [DependsOn(typeof(FileManagementMongoDbModule))]
    [DependsOn(typeof(FormsMongoDbModule))]
    [DependsOn(typeof(CmsKitProMongoDbModule))]
    [DependsOn(typeof(AbpGdprMongoDbModule))]
    public class LeptonXDemoAppMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<LeptonXDemoAppMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
        }
    }
}
