using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.ObjectExtending;

public class FileManagementModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public FileManagementModuleExtensionConfiguration ConfigureDirectoryDescriptor(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            FileManagementModuleExtensionConsts.EntityNames.DirectoryDescriptor,
            configureAction
        );
    }

    public FileManagementModuleExtensionConfiguration ConfigureFileDescriptor(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            FileManagementModuleExtensionConsts.EntityNames.FileDescriptor,
            configureAction
        );
    }
}
