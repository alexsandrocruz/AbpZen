using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Chat;

[DependsOn(
    typeof(ChatApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ChatHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Chat";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(ChatApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ChatHttpApiClientModule>();
        });
    }
}
