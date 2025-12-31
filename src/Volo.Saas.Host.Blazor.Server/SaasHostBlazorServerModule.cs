using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.FeatureManagement.Blazor.Server;
using Volo.Abp.Modularity;

namespace Volo.Saas.Host.Blazor.Server;

[DependsOn(
    typeof(SaasHostBlazorModule),
    typeof(AbpFeatureManagementBlazorServerModule)
)]
public class SaasHostBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                BlazorStandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(SaasBundleContributor));
                }
            );
        });
    }
}
