using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Payment.Plans;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpPaymentDomainSharedModule)
)]
public class AbpPaymentDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.Plan,
            typeof(Plan)
        );

        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
            PaymentModuleExtensionConsts.ModuleName,
            PaymentModuleExtensionConsts.EntityNames.GatewayPlan,
            typeof(GatewayPlan)
        );
    }
}
