using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.LeptonXTheme;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpLeptonXThemeInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLeptonXThemeInstallerModule>();
        });
    }
}