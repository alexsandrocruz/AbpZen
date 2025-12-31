using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public class PaymentModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public PaymentModuleExtensionConfiguration ConfigurePlan(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            PaymentModuleExtensionConsts.EntityNames.Plan,
            configureAction
        );
    }

    public PaymentModuleExtensionConfiguration ConfigureGatewayPlan(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            PaymentModuleExtensionConsts.EntityNames.GatewayPlan,
            configureAction
        );
    }
}
