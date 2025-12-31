namespace Volo.CmsKit;

public static class CmsKitProSettingNames
{
    private const string DefaultPrefix = "Volo.CmsKitPro";

    public static class Contact
    {
        private const string ContactPrefix = DefaultPrefix + ".Contact";

        public const string ReceiverEmailAddress = ContactPrefix + "ReceiverEmailAddress";
    }
    
    public static class PageFeedback
    {
        private const string PageFeedbackPrefix = DefaultPrefix + ".PageFeedback";

        public const string IsAutoHandled = PageFeedbackPrefix + "IsAutoHandled";
        
        public const string RequireCommentsForNegativeFeedback = PageFeedbackPrefix + "RequireCommentsForNegativeFeedback";
    }
}
