using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Features;
public class CmsKitProFeatureDefinitionProvider : CmsKitFeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(CmsKitProFeatures.GroupName,
            L("Feature:CmsKitProGroup"));

        if (GlobalFeatureManager.Instance.IsEnabled<ContactFeature>())
        {
            group.AddFeature(CmsKitProFeatures.ContactEnable,
            "true",
            L("Feature:ContactEnable"),
            L("Feature:ContactEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<NewslettersFeature>())
        {
            group.AddFeature(CmsKitProFeatures.NewsletterEnable,
            "true",
            L("Feature:NewsletterEnable"),
            L("Feature:NewsletterEnableDescription"),
            new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<PollsFeature>())
        {
            group.AddFeature(CmsKitProFeatures.PollEnable,
           "true",
           L("Feature:PollEnable"),
           L("Feature:PollEnableDescription"),
           new ToggleStringValueType());
        }

        if (GlobalFeatureManager.Instance.IsEnabled<UrlShortingFeature>())
        {
            group.AddFeature(CmsKitProFeatures.UrlShortingEnable,
           "true",
           L("Feature:UrlShortingEnable"),
           L("Feature:UrlShortingEnableDescription"),
           new ToggleStringValueType());
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<PageFeedbackFeature>())
        {
            group.AddFeature(CmsKitProFeatures.PageFeedbackEnable,
           "true",
           L("Feature:PageFeedbackEnable"),
           L("Feature:PageFeedbackEnableDescription"),
           new ToggleStringValueType());
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<FaqFeature>())
        {
            group.AddFeature(CmsKitProFeatures.FaqEnable,
           "true",
           L("Feature:FaqEnable"),
           L("Feature:FaqEnableDescription"),
           new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}