using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpOpenIddictProInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictProInstallerModule>();
        });
    }
}
