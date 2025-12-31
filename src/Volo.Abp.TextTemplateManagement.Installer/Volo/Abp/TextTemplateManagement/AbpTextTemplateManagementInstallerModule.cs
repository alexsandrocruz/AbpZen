using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplateManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpTextTemplateManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTextTemplateManagementInstallerModule>();
        });
    }
}
