using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class FaqFeature: GlobalFeature
{
    public const string Name = "CmsKitPro.Faq";

    internal FaqFeature(
        [NotNull] GlobalCmsKitProFeatures cmsKitPro
    ) : base(cmsKitPro)
    {
    }
}
