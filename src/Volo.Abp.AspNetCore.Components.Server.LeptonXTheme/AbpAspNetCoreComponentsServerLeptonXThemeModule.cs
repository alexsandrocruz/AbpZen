using Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AspNetCore.Components.Server.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebLeptonXThemeModule),
        typeof(AbpAspNetCoreComponentsServerThemingModule)
        )]
    public class AbpAspNetCoreComponentsServerLeptonXThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new LeptonXThemeBlazorServerToolbarContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(BlazorLeptonXThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(BlazorStandardBundles.Styles.Global)
                            .AddContributors(typeof(BlazorLeptonXThemeStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(BlazorLeptonXThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(BlazorStandardBundles.Scripts.Global)
                            .AddContributors(typeof(BlazorLeptonXThemeScriptContributor));
                    });
            });
        }
    }
}