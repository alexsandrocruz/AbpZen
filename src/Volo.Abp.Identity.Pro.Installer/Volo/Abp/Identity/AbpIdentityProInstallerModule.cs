using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpIdentityProInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityProInstallerModule>();
        });
    }
}
