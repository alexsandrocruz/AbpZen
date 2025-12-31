using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Commercial;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Gdpr.Web.Navigation;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Gdpr.Web;

[DependsOn(
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeCommercialModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
)]
public class AbpGdprWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpGdprWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpGdprWebMainMenuContributor());
        });
        
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGdprWebModule>("Volo.Abp.Gdpr.Web");
        });
        
        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(GdprRemoteServiceConsts.ModuleName);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}