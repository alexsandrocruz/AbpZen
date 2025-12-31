using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Sms.Twilio;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
)]
public class AbpTwilioSmsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTwilioSmsInstallerModule>();
        });
    }
}
