using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public class CmsKitProModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public CmsKitProModuleExtensionConfiguration ConfigureNewsletterRecord(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitProModuleExtensionConsts.EntityNames.NewsletterRecord,
            configureAction
        );
    }

    public CmsKitProModuleExtensionConfiguration ConfigurePoll(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitProModuleExtensionConsts.EntityNames.Poll,
            configureAction
        );
    }
    public CmsKitProModuleExtensionConfiguration ConfigureFaq(
    Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            CmsKitProModuleExtensionConsts.EntityNames.Faq,
            configureAction
        );
    }
}