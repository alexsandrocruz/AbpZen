using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.ObjectExtending;

public static class FileManagementModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureFileManagement(
        this ModuleExtensionConfigurationDictionary modules,
        Action<FileManagementModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            FileManagementModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
