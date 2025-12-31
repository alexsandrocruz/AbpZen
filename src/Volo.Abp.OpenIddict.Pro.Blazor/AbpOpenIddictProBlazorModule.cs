using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AuditLogging.Blazor;
using Volo.Abp.OpenIddict.Pro.Blazor.Menus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.PermissionManagement.Blazor;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict.Pro.Blazor;

[DependsOn(
    typeof(AbpOpenIddictProApplicationContractsModule),
    typeof(AbpPermissionManagementBlazorModule),
    typeof(AbpAutoMapperModule),
     typeof(AbpAuditLoggingBlazorModule)
    )]
public class AbpOpenIddictProBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpOpenIddictProBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpOpenIddictProBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new OpenIddictProMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpOpenIddictProBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
