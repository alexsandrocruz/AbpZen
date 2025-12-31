using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.LanguageManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpLanguageManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpLanguageManagementInstallerModule>();
        });
    }
}
