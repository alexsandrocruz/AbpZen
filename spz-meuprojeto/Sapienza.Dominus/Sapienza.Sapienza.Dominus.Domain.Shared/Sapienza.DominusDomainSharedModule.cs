using Sapienza.Sapienza.Dominus.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.GlobalFeatures;
using Volo.Chat;
using Volo.CmsKit;
using Volo.FileManagement;
using Volo.Forms;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;

namespace Sapienza.Sapienza.Dominus
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpIdentityProDomainSharedModule),
        typeof(AbpOpenIddictDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(LanguageManagementDomainSharedModule),
        typeof(SaasDomainSharedModule),
        typeof(TextTemplateManagementDomainSharedModule),
        typeof(LeptonThemeManagementDomainSharedModule),
        typeof(AbpGlobalFeaturesModule),
        typeof(BlobStoringDatabaseDomainSharedModule)
        )]
    [DependsOn(typeof(ChatDomainSharedModule))]
    [DependsOn(typeof(FileManagementDomainSharedModule))]
    [DependsOn(typeof(FormsDomainSharedModule))]
    [DependsOn(typeof(CmsKitProDomainSharedModule))]
    [DependsOn(typeof(AbpGdprDomainSharedModule))]
    public class Sapienza.DominusDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            Sapienza.DominusGlobalFeatureConfigurator.Configure();
            Sapienza.DominusModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<Sapienza.DominusDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<Sapienza.DominusResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Sapienza.Sapienza.Dominus");

                options.DefaultResourceType = typeof(Sapienza.DominusResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Sapienza.Sapienza.Dominus", typeof(Sapienza.DominusResource));
            });
        }
    }
}
