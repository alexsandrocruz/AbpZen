using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.LeptonX.Shared;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpAspNetCoreLeptonXSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAspNetCoreLeptonXSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<LeptonXResource>("en")
                .AddVirtualJson("/Localization/LeptonX");
        });
    }
}
