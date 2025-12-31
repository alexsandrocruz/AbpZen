using Volo.Abp.Account.Pro.Public.Blazor.Shared;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Account.Pro.Public.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(AbpAccountSharedApplicationContractsModule),
    typeof(AbpAccountPublicBlazorSharedModule)
)]
public class AbpAccountPublicBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AccountBlazorToolbarContributor());
        });
        
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AccountBlazorUserMenuContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                BlazorStandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(AbpAccountBlazorBundleContributor));
                }
            );
        });
    }
}
