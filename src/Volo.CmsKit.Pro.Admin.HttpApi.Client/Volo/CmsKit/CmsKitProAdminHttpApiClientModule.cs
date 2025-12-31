using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Admin;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProAdminApplicationContractsModule),
    typeof(CmsKitAdminHttpApiClientModule),
    typeof(CmsKitProCommonHttpApiClientModule)
    )]
public class CmsKitProAdminHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(CmsKitProAdminApplicationContractsModule).Assembly,
            CmsKitAdminRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProAdminHttpApiClientModule>();
        });
    }
}
