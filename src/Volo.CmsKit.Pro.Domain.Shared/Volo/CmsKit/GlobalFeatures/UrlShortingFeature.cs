using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class UrlShortingFeature : GlobalFeature
{
    public const string Name = "CmsKitPro.UrlShorting";

    internal UrlShortingFeature(
        [NotNull] GlobalCmsKitProFeatures cmsKitPro
    ) : base(cmsKitPro)
    {

    }
}
