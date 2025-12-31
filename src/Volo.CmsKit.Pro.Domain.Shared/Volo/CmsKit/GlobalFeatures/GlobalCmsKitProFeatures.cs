using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

public class GlobalCmsKitProFeatures : GlobalModuleFeatures
{
    public const string ModuleName = "CmsKitPro";

    public NewslettersFeature Newsletter => GetFeature<NewslettersFeature>();

    public ContactFeature Contact => GetFeature<ContactFeature>();

    public UrlShortingFeature UrlShortingFeature => GetFeature<UrlShortingFeature>();

    public PollsFeature PollsFeature => GetFeature<PollsFeature>();
    
    public PageFeedbackFeature PageFeedbackFeature => GetFeature<PageFeedbackFeature>();
    public FaqFeature FaqFeature => GetFeature<FaqFeature>();

    public GlobalCmsKitProFeatures([NotNull] GlobalFeatureManager featureManager)
        : base(featureManager)
    {
        AddFeature(new NewslettersFeature(this));
        AddFeature(new ContactFeature(this));
        AddFeature(new UrlShortingFeature(this));
        AddFeature(new PollsFeature(this));
        AddFeature(new PageFeedbackFeature(this));
        AddFeature(new FaqFeature(this));
    }
}
