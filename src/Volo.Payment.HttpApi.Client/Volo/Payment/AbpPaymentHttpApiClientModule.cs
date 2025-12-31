using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpPaymentApplicationContractsModule)
    )]
public class AbpPaymentHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpPaymentApplicationContractsModule).Assembly,
            AbpPaymentCommonRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentHttpApiClientModule>();
        });
    }
}
