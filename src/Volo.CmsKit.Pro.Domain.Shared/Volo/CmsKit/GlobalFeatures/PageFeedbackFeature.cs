using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class PageFeedbackFeature : GlobalFeature
{
    public const string Name = "CmsKitPro.PageFeedback";
    
    public PageFeedbackFeature([NotNull] GlobalCmsKitProFeatures module) : base(module)
    {
    }
}