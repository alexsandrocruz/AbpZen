using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement.Blazor;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Host.Blazor.Navigation;
using Volo.Saas.Host.Dtos;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Blazor;

[DependsOn(
    typeof(SaasHostApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFeatureManagementBlazorModule)
    )]
public class SaasHostBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SaasHostBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SaasHostBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SaasHostMainMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SaasHostBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SaasResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    SaasModuleExtensionConsts.ModuleName,
                    SaasModuleExtensionConsts.EntityNames.Tenant,
                    createFormTypes: new[] { typeof(SaasTenantCreateDto) },
                    editFormTypes: new[] { typeof(SaasTenantUpdateDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    SaasModuleExtensionConsts.ModuleName,
                    SaasModuleExtensionConsts.EntityNames.Edition,
                    createFormTypes: new[] { typeof(EditionCreateDto) },
                    editFormTypes: new[] { typeof(EditionUpdateDto) }
                );
        });
    }
}
