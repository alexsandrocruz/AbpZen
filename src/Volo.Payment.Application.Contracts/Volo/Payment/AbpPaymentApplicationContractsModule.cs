using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Payment.Plans;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpPaymentDomainSharedModule),
    typeof(AbpDddApplicationContractsModule))]
public class AbpPaymentApplicationContractsModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.Plan,
            getApiTypes: new[] { typeof(PlanDto) }
        );

        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.GatewayPlan,
            getApiTypes: new[] { typeof(GatewayPlanDto) }
        );
    }
}
