using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Chat.Blazor.Server;

[DependsOn(
    typeof(ChatBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class ChatBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                BlazorStandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(ChatBundleContributor));
                }
            );
        });
    }
}
