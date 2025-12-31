using Volo.Abp.Settings;

namespace Volo.CmsKit;

public class CmsKitProSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(CmsKitProSettingNames.Contact.ReceiverEmailAddress, "info@mycompanyname.com", null, null, true));
        context.Add(new SettingDefinition(CmsKitProSettingNames.PageFeedback.IsAutoHandled, true.ToString(), null, null, true));
        context.Add(new SettingDefinition(CmsKitProSettingNames.PageFeedback.RequireCommentsForNegativeFeedback, false.ToString(), null, null, true));
    }
}
