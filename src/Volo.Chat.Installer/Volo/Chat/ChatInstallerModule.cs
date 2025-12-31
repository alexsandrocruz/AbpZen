using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Chat;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ChatInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ChatInstallerModule>();
        });
    }
}
