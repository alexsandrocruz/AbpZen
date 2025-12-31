using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.FileManagement.Blazor.Server.Bundling;

namespace Volo.FileManagement.Blazor.Server;

[DependsOn(
    typeof(FileManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
)]
public class FileManagementBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BlazorStandardBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(FileManagementStyleContributor));
                }
            );
        });
    }
}
