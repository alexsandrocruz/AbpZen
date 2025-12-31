using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Forms;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(FormsApplicationContractsModule)
    )]
public class FormsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(FormsApplicationContractsModule).Assembly,
            FormsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FormsHttpApiClientModule>();
        });
    }
}
