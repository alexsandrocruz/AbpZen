using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProPublicApplicationContractsModule),
    typeof(CmsKitPublicHttpApiClientModule),
    typeof(CmsKitProCommonHttpApiClientModule)
    )]
public class CmsKitProPublicHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(CmsKitProPublicApplicationContractsModule).Assembly,
            CmsKitProCommonRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProPublicHttpApiClientModule>();
        });
    }
}
