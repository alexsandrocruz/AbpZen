using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.CmsKit;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CmsKitProInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProInstallerModule>();
        });
    }
}
