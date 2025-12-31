using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;
using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsMauiBlazorThemingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAccountPublicHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelMauiBlazorModule)
)]
public class AbpAccountPublicMauiBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorizationCore();
        context.Services.AddSingleton<AuthenticationStateProvider, ExternalAuthStateProvider>();

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpAccountPublicMauiBlazorModule).Assembly);
        });
        
        context.Services.AddHttpClient(AccessTokenStore.HttpClientName);
    }
}
