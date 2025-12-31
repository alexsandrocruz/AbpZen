using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictProApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpOpenIddictProHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpOpenIddictProApplicationContractsModule).Assembly,
            AbpOpenIddictProRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictProHttpApiClientModule>();
        });
    }
}
