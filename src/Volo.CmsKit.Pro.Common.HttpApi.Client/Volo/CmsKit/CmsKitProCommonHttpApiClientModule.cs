using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitCommonHttpApiClientModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProCommonHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(CmsKitProCommonApplicationContractsModule).Assembly,
            CmsKitProCommonRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProCommonHttpApiClientModule>();
        });
    }
}
