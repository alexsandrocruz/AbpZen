using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AuditLogging;

[DependsOn(
    typeof(AbpAuditLoggingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpAuditLoggingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpAuditLoggingApplicationContractsModule).Assembly,
            AuditLoggingRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAuditLoggingHttpApiClientModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
