using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
#if NIGHTLY
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
#endif
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAspNetCoreLeptonXSharedModule)
        )]
    public class AbpAspNetCoreComponentsWebLeptonXThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpAspNetCoreComponentsWebLeptonXThemeModule>();
            ConfigureToolbarOptions();
            ConfigureRouterOptions();
            ConfigurePageHeaderOptions();
            ConfigureLeptonXStyles();
            ConfigureLeptonXTheme();
        }

        private void ConfigureToolbarOptions()
        {
            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new LeptonXThemeWebToolbarContributor());
            });
        }

        private void ConfigureRouterOptions()
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsWebLeptonXThemeModule).Assembly);
            });
        }

        private void ConfigureLeptonXStyles()
        {
            Configure<LeptonXThemeOptions>(o =>
            {
                o.Styles[LeptonXStyleNames.Light] =
                    new LeptonXThemeStyle(L("Theme:" + LeptonXStyleNames.Light), "bi bi-sun-fill");

                o.Styles[LeptonXStyleNames.Dim] =
                    new LeptonXThemeStyle(L("Theme:" + LeptonXStyleNames.Dim), "bi bi-brightness-alt-high-fill");

                o.Styles[LeptonXStyleNames.Dark] =
                    new LeptonXThemeStyle(L("Theme:" + LeptonXStyleNames.Dark), "bi bi-moon-fill");
                
                o.Styles[LeptonXStyleNames.System] =
                    new LeptonXThemeStyle(L("Theme:" + LeptonXStyleNames.System), "bi bi-laptop-fill");
            });
        }

        private void ConfigurePageHeaderOptions()
        {
            Configure<PageHeaderOptions>(options =>
            {
                options.RenderPageTitle = false;
                options.RenderBreadcrumbs = false;
                options.RenderToolbar = false;
            });
        }
        
        private void ConfigureLeptonXTheme()
        {
            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<LeptonXTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = LeptonXTheme.Name;
                }
            });
        }

        private static LocalizableString L(string key)
        {
            return LocalizableString.Create<LeptonXResource>(key);
        }
    }
}