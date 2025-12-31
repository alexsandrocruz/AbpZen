using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public static class PaymentModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigurePayment(
        this ModuleExtensionConfigurationDictionary modules,
        Action<PaymentModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            PaymentModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
