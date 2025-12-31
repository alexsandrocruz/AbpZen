using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.Server;

namespace Volo.CmsKit.Pro.Admin.Blazor.Server;

[DependsOn(
    typeof(AbpSettingManagementBlazorServerModule),
    typeof(CmsKitProAdminBlazorModule)
    )]
public class CmsKitProAdminBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BlazorStandardBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(CmsKitProStyleContributor));
                }
            );
            options.ScriptBundles.Configure(
                BlazorStandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(CmsKitProScriptContributor));
                }
            );
        });
    }
}
