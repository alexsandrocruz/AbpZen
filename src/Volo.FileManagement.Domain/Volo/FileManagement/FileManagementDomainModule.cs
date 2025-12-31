using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;
using Volo.ObjectExtending;

namespace Volo.FileManagement;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(FileManagementDomainSharedModule),
    typeof(AbpAutoMapperModule)
)]
public class FileManagementDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FileManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<FileManagementDomainMappingProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<FileDescriptor, FileDescriptorEto>(typeof(FileManagementDomainModule));
            options.EtoMappings.Add<DirectoryDescriptor, DirectoryDescriptorEto>(typeof(FileManagementDomainModule));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                FileManagementModuleExtensionConsts.ModuleName,
                FileManagementModuleExtensionConsts.EntityNames.DirectoryDescriptor,
                typeof(DirectoryDescriptor)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                FileManagementModuleExtensionConsts.ModuleName,
                FileManagementModuleExtensionConsts.EntityNames.FileDescriptor,
                typeof(FileDescriptor)
            );
        });
    }
}
