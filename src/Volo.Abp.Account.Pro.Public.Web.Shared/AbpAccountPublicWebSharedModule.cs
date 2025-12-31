using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Pro.Public.Web.Shared;

[DependsOn(typeof(AbpAccountPublicApplicationContractsModule))]
[DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
public class AbpAccountPublicWebSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AccountResource),
                typeof(AbpAccountPublicApplicationContractsModule).Assembly,
                typeof(AbpAccountPublicWebSharedModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountPublicWebSharedModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountPublicWebSharedModule>();
        });
        
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                StandardBundles.Scripts.Global,
                configuration =>
                {
                    configuration.AddFiles("/client-proxies/account-proxy.js");
                    configuration.AddContributors(new AbpAccountPublicWebSharedBundleContributor());
                }
            );
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AccountProPublicRemoteServiceConsts.ModuleName);
        });
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}