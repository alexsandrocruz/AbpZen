using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.StaticProxying;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account;

[DependsOn(
    typeof(AbpAccountPublicApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpAccountPublicHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpAccountPublicApplicationContractsModule).Assembly,
            AccountProPublicRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountPublicHttpApiClientModule>();
        });

        Configure<AbpHttpClientStaticProxyingOptions>(options =>
        {
            options.BindingFromQueryTypes.Add(typeof(GetIdentitySecurityLogListInput));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
