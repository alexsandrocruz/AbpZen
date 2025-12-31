using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Admin.GlobalResources;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Admin.Tags;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitAdminApplicationContractsModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProAdminApplicationContractsModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitProModuleExtensionConsts.ModuleName,
                CmsKitProModuleExtensionConsts.EntityNames.NewsletterRecord,
                getApiTypes: new[] { typeof(NewsletterRecordDto), typeof(NewsletterRecordWithDetailsDto) }
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitProModuleExtensionConsts.ModuleName,
                CmsKitProModuleExtensionConsts.EntityNames.Poll,
                getApiTypes: new[] { typeof(PollDto), typeof(PollWithDetailsDto) },
                createApiTypes: new[] { typeof(CreatePollDto) },
                updateApiTypes: new[] { typeof(UpdatePollDto) }
            );
        });
    }
}
