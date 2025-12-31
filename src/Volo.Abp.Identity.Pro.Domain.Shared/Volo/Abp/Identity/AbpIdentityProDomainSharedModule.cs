using Volo.Abp.Features;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Ldap;

namespace Volo.Abp.Identity;

[DependsOn(
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpLocalizationModule),
    typeof(AbpFeaturesModule),
    typeof(AbpSettingsModule),
    typeof(AbpLdapAbstractionsModule)
    )]
public class AbpIdentityProDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityProDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddVirtualJson("/Volo/Abp/Identity/Localization/DomainShared")
                .AddVirtualJson("/Volo/Abp/Identity/Localization/ApplicationContracts");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Volo.Abp.Identity", typeof(IdentityResource));
        });
    }
}
