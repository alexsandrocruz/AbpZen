using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.FileManagement;

[DependsOn(
    typeof(FileManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class FileManagementHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "FileManagement";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(FileManagementApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FileManagementHttpApiClientModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
