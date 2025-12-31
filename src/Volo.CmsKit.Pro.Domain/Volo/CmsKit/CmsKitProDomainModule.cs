using Volo.Abp;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitProDomainSharedModule),
    typeof(CmsKitDomainModule),
    typeof(AbpEmailingModule),
    typeof(AbpTextTemplatingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpSettingManagementDomainModule)
)]
public class CmsKitProDomainModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProDomainModule>();
        });
    }
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                CmsKitProModuleExtensionConsts.ModuleName,
                CmsKitProModuleExtensionConsts.EntityNames.NewsletterRecord,
                typeof(NewsletterRecord)
            );
            
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                CmsKitProModuleExtensionConsts.ModuleName,
                CmsKitProModuleExtensionConsts.EntityNames.Poll,
                typeof(Poll)
            );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
