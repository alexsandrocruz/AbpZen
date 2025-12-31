using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpLocalizationModule)
)]
public class AbpOpenIddictProDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictProDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("Volo/Abp/OpenIddict/Localization/Resources/OpenIddict/ApplicationContracts");
        });
    }

}
