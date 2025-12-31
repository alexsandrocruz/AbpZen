using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Payment.Admin.Blazor.Navigation;
using Volo.Payment.Admin.Plans;
using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Blazor;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpPaymentAdminApplicationContractsModule)
    )]
public class AbpPaymentAdminBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpPaymentAdminBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpPaymentAdminBlazorAutoMapperProfile>();
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpPaymentAdminBlazorMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpPaymentAdminBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PaymentResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToUi(
                PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.Plan,
                createFormTypes: new[] { typeof(PlanCreateInput) },
                editFormTypes: new[] { typeof(PlanUpdateInput) }
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToUi(
                PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.GatewayPlan,
                createFormTypes: new[] { typeof(GatewayPlanCreateInput) },
                editFormTypes: new[] { typeof(GatewayPlanUpdateInput) }
            );
        });
    }
}
