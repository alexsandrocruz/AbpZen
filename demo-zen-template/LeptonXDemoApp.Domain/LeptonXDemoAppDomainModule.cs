using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LeptonXDemoApp.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Saas;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Commercial.SuiteTemplates;
using Volo.Chat;
using Volo.CmsKit;
using Volo.FileManagement;
using Volo.Abp.BlobStoring;
using Volo.Forms;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;

namespace LeptonXDemoApp
{
    [DependsOn(
        typeof(LeptonXDemoAppDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityProDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpOpenIddictDomainModule),
        typeof(AbpPermissionManagementDomainOpenIddictModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(SaasDomainModule),
        typeof(TextTemplateManagementDomainModule),
        typeof(LeptonThemeManagementDomainModule),
        typeof(LanguageManagementDomainModule),
        typeof(VoloAbpCommercialSuiteTemplatesModule),
        typeof(AbpEmailingModule),
        typeof(BlobStoringDatabaseDomainModule)
        )]
    [DependsOn(typeof(ChatDomainModule))]
    [DependsOn(typeof(FileManagementDomainModule))]
    [DependsOn(typeof(FormsDomainModule))]
    [DependsOn(typeof(CmsKitProDomainModule))]
    [DependsOn(typeof(AbpGdprDomainModule))]
    public class LeptonXDemoAppDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português (Brasil)"));
            });

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseDatabase();
                });

                options.Containers.Configure<FileManagementContainer>(c =>
                {
                    c.UseDatabase(); // You can use FileSystem or Azure providers also.
                });
            });

#if DEBUG
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
        }
    }
}
