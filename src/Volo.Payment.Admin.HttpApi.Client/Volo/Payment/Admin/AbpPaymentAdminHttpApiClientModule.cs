using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Payment.Admin;

[DependsOn(
    typeof(AbpPaymentAdminApplicationContractsModule),
    typeof(AbpPaymentHttpApiClientModule)
    )]
public class AbpPaymentAdminHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpPaymentAdminApplicationContractsModule).Assembly,
            AbpPaymentAdminRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPaymentAdminHttpApiClientModule>();
        });
    }
}
