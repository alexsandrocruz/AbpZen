using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Newsletters;

public static class NewsletterPreferenceConst
{
    public static int MaxPreferenceLength { get; set; } = CmsEntityConsts.MaxPreferenceLength;

    public static int MaxSourceLength { get; set; } = CmsEntityConsts.MaxSourceLength;

    public static int MaxSourceUrlLength { get; set; } = 256;
}
