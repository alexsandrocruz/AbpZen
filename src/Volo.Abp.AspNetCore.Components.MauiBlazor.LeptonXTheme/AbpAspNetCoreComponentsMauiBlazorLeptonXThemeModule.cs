using Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Navigation;
using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client.IdentityModel.MauiBlazor;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsMauiBlazorThemingModule),
        typeof(AbpAspNetCoreComponentsWebLeptonXThemeModule),
        typeof(AbpHttpClientIdentityModelMauiBlazorModule)
        )]
    public class AbpAspNetCoreComponentsMauiBlazorLeptonXThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsMauiBlazorLeptonXThemeModule).Assembly);
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new LeptonXThemeToolbarContributor());
            });

            context.Services.AddAutoMapperObjectMapper<AbpAspNetCoreComponentsMauiBlazorLeptonXThemeModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpAspNetCoreComponentsMauiBlazorLeptonXThemeModule>();
            });
            
            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Remove(typeof(Web.LeptonXTheme.LeptonXTheme));
                options.Themes.Add<LeptonXMauiBlazorTheme>();
    
                if (options.DefaultThemeName == Web.LeptonXTheme.LeptonXTheme.Name || options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = LeptonXMauiBlazorTheme.Name;
                }
            });
        }
        
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context)) ;
        }
    
        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await context.ServiceProvider.GetRequiredService<LanguageMauiBlazorManager>().InitializeAsync();
        }
    }
}
