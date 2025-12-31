using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.ObjectMapping;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;

[DependsOn(
    typeof(AbpAspNetCoreLeptonXSharedModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAutoMapperModule)
)]
public class AbpAspNetCoreMvcUiLeptonXThemeModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAspNetCoreMvcUiLeptonXThemeModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLeptonXStyles();

        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<LeptonXTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = LeptonXTheme.Name;
            }
        });

        Configure<AbpErrorPageOptions>(options =>
        {
            options.ErrorViewUrls.Add("401", "~/Views/Error/401.cshtml");
            options.ErrorViewUrls.Add("403", "~/Views/Error/403.cshtml");
            options.ErrorViewUrls.Add("404", "~/Views/Error/404.cshtml");
            options.ErrorViewUrls.Add("500", "~/Views/Error/500.cshtml");
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiLeptonXThemeModule>();
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new LeptonXThemeMainTopToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(LeptonXThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(StandardBundles.Styles.Global)
                        .AddContributors(typeof(LeptonXGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(LeptonXThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(StandardBundles.Scripts.Global)
                        .AddContributors(typeof(LeptonXGlobalScriptContributor));
                });
        });

        context.Services.AddAutoMapperObjectMapper<AbpAspNetCoreMvcUiLeptonXThemeModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<LeptonXThemeAutoMapperProfile>(validate: true);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
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

    private static LocalizableString L(string key)
    {
        return LocalizableString.Create<LeptonXResource>(key);
    }
}