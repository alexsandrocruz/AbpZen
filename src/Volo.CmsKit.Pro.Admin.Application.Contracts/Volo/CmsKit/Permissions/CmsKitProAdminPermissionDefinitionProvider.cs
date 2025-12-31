using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions;

public class CmsKitProAdminPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cmsGroup = context.GetGroupOrNull(CmsKitProAdminPermissions.GroupName)
                       ?? context.AddGroup(CmsKitProAdminPermissions.GroupName, L("Permission:CmsKit"));

        var newsletterGroup = cmsGroup.AddPermission(CmsKitProAdminPermissions.Newsletters.Default, L("Permission:NewsletterManagement"))
            .RequireGlobalFeatures(typeof(NewslettersFeature))
            .RequireFeatures(CmsKitProFeatures.NewsletterEnable);
        newsletterGroup.AddChild(CmsKitProAdminPermissions.Newsletters.EditPreferences, L("Permission:Newsletter.EditPreferences"))
            .RequireGlobalFeatures(typeof(NewslettersFeature))
            .RequireFeatures(CmsKitProFeatures.NewsletterEnable);
        newsletterGroup.AddChild(CmsKitProAdminPermissions.Newsletters.Import, L("Permission:Newsletter.Import"))
            .RequireGlobalFeatures(typeof(NewslettersFeature))
            .RequireFeatures(CmsKitProFeatures.NewsletterEnable);

        var pollGroup = cmsGroup.AddPermission(CmsKitProAdminPermissions.Polls.Default, L("Permission:Poll"))
            .RequireGlobalFeatures(typeof(PollsFeature))
            .RequireFeatures(CmsKitProFeatures.PollEnable);
        pollGroup.AddChild(CmsKitProAdminPermissions.Polls.Create, L("Permission:Poll.Create"))
            .RequireGlobalFeatures(typeof(PollsFeature))
             .RequireFeatures(CmsKitProFeatures.PollEnable);
        pollGroup.AddChild(CmsKitProAdminPermissions.Polls.Update, L("Permission:Poll.Update"))
            .RequireGlobalFeatures(typeof(PollsFeature))
            .RequireFeatures(CmsKitProFeatures.PollEnable);
        pollGroup.AddChild(CmsKitProAdminPermissions.Polls.Delete, L("Permission:Poll.Delete"))
            .RequireGlobalFeatures(typeof(PollsFeature))
            .RequireFeatures(CmsKitProFeatures.PollEnable);

        cmsGroup.AddPermission(CmsKitProAdminPermissions.Contact.SettingManagement, L("Permission:Contact:SettingManagement"))
            .RequireGlobalFeatures(typeof(ContactFeature))
            .RequireFeatures(CmsKitProFeatures.ContactEnable);

        var urlShortingGroup = cmsGroup.AddPermission(CmsKitProAdminPermissions.UrlShorting.Default, L("Permission:UrlShorting"))
            .RequireGlobalFeatures(typeof(UrlShortingFeature))
            .RequireFeatures(CmsKitProFeatures.UrlShortingEnable);
        urlShortingGroup.AddChild(CmsKitProAdminPermissions.UrlShorting.Create, L("Permission:UrlShorting.Create"))
            .RequireGlobalFeatures(typeof(UrlShortingFeature))
            .RequireFeatures(CmsKitProFeatures.UrlShortingEnable);
        urlShortingGroup.AddChild(CmsKitProAdminPermissions.UrlShorting.Update, L("Permission:UrlShorting.Update"))
            .RequireGlobalFeatures(typeof(UrlShortingFeature))
            .RequireFeatures(CmsKitProFeatures.UrlShortingEnable);
        urlShortingGroup.AddChild(CmsKitProAdminPermissions.UrlShorting.Delete, L("Permission:UrlShorting.Delete"))
            .RequireGlobalFeatures(typeof(UrlShortingFeature))
            .RequireFeatures(CmsKitProFeatures.UrlShortingEnable);
        
        var pageFeedbackGroup = cmsGroup.AddPermission(CmsKitProAdminPermissions.PageFeedbacks.Default, L("Permission:PageFeedback"))
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable);
        pageFeedbackGroup.AddChild(CmsKitProAdminPermissions.PageFeedbacks.Update, L("Permission:PageFeedback.Update"))
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable);
        pageFeedbackGroup.AddChild(CmsKitProAdminPermissions.PageFeedbacks.Delete, L("Permission:PageFeedback.Delete"))
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable);
        pageFeedbackGroup.AddChild(CmsKitProAdminPermissions.PageFeedbacks.Settings, L("Permission:PageFeedback.Mail.Settings"))
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable);
        pageFeedbackGroup.AddChild(CmsKitProAdminPermissions.PageFeedbacks.SettingManagement, L("Permission:PageFeedback.SettingManagement"))
            .RequireGlobalFeatures(typeof(PageFeedbackFeature))
            .RequireFeatures(CmsKitProFeatures.PageFeedbackEnable);

        var faqGroup = cmsGroup.AddPermission(CmsKitProAdminPermissions.Faqs.Default, L("Permission:Faq"))
            .RequireGlobalFeatures(typeof(FaqFeature))
            .RequireFeatures(CmsKitProFeatures.FaqEnable);
        faqGroup.AddChild(CmsKitProAdminPermissions.Faqs.Create, L("Permission:Faq.Create"))
            .RequireGlobalFeatures(typeof(FaqFeature))
             .RequireFeatures(CmsKitProFeatures.FaqEnable);
        faqGroup.AddChild(CmsKitProAdminPermissions.Faqs.Update, L("Permission:Faq.Update"))
            .RequireGlobalFeatures(typeof(FaqFeature))
            .RequireFeatures(CmsKitProFeatures.FaqEnable);
            faqGroup.AddChild(CmsKitProAdminPermissions.Faqs.Delete, L("Permission:Faq.Delete"))
            .RequireGlobalFeatures(typeof(FaqFeature))
            .RequireFeatures(CmsKitProFeatures.FaqEnable);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}
