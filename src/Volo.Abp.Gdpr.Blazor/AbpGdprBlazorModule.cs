using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Gdpr.Blazor.Navigation;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Abp.Gdpr.Localization;

namespace Volo.Abp.Gdpr.Blazor;

[DependsOn(
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule)
    )]
public class AbpGdprBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpGdprBlazorAutoMapperProfile>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpGdprBlazorAutoMapperProfile>(validate: true);
        });
        
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpGdprBlazorMainMenuContributor());
        });
        
        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpGdprBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpGdprResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}