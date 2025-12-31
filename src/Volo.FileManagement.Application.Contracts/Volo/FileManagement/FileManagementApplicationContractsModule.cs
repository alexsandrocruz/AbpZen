using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Authorization;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;
using Volo.ObjectExtending;

namespace Volo.FileManagement;

[DependsOn(
    typeof(FileManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class FileManagementApplicationContractsModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FileManagementApplicationContractsModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    FileManagementModuleExtensionConsts.ModuleName,
                    FileManagementModuleExtensionConsts.EntityNames.DirectoryDescriptor,
                    getApiTypes: new[] { typeof(DirectoryDescriptorDto) },
                    createApiTypes: new[] { typeof(CreateDirectoryInput) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    FileManagementModuleExtensionConsts.ModuleName,
                    FileManagementModuleExtensionConsts.EntityNames.FileDescriptor,
                    getApiTypes: new[] { typeof(FileDescriptorDto) },
                    createApiTypes: new[] { typeof(CreateFileInputWithStream) }
                );
        });
    }
}
