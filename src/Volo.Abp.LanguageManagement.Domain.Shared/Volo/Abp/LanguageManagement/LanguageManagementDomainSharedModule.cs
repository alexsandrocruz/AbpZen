using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.LanguageManagement;

[DependsOn(
    typeof(AbpLocalizationModule),
    typeof(AbpValidationModule),
    typeof(AbpFeaturesModule)
    )]
public class LanguageManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<LanguageManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<LanguageManagementResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Volo/Abp/LanguageManagement/Localization/Resources")
                .AddVirtualJson("/Volo/Abp/LanguageManagement/Localization/ApplicationContracts");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Volo.Abp.LanguageManagement", typeof(LanguageManagementResource));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
