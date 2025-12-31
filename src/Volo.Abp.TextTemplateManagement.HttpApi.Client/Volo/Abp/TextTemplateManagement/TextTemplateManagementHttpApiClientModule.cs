using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplateManagement;

[DependsOn(
    typeof(TextTemplateManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TextTemplateManagementHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "TextTemplateManagement";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(TextTemplateManagementApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TextTemplateManagementHttpApiClientModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
