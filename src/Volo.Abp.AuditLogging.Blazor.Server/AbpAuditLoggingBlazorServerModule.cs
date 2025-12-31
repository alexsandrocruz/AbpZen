using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.Blazor.Server;

[DependsOn(typeof(AbpAuditLoggingBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule))]
public class AbpAuditLoggingBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                BlazorStandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(AuditLoggingScriptBundleContributor));
                }
            );

            options.StyleBundles.Configure(
                BlazorStandardBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(AuditLoggingStyleBundleContributor));
                }
            );
        });
    }
}
