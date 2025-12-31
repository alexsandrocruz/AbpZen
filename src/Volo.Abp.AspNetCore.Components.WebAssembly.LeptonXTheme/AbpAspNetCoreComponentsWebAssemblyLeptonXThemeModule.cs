using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpAspNetCoreComponentsWebLeptonXThemeModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule)
    )]
    public class AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule).Assembly);
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new LeptonXThemeToolbarContributor());
            });

            context.Services.AddAutoMapperObjectMapper<AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule>();
            });

            if (context.Services.ExecutePreConfiguredActions<AbpAspNetCoreComponentsWebOptions>().IsBlazorWebApp)
            {
                Configure<AuthenticationOptions>(options =>
                {
                    options.LoginUrl = "Account/Login";
                    options.LogoutUrl = "Account/Logout";
                });
            }
        }
    }
}
