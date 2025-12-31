using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Payment;

namespace Volo.Saas.Host;

[DependsOn(
    typeof(SaasHostApplicationContractsModule),
    typeof(AbpHttpClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule)
    )]
public class SaasHostHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(SaasHostApplicationContractsModule).Assembly,
            SaasHostRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SaasHostHttpApiClientModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
