using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public static class CmsKitProModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureCmsKitPro(
        this ModuleExtensionConfigurationDictionary modules,
        Action<CmsKitProModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            CmsKitProModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}