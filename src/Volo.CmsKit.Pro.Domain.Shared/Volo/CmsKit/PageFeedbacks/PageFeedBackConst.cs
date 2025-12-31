using JetBrains.Annotations;
using Volo.CmsKit.Entities;

namespace Volo.CmsKit.PageFeedbacks;

public static class PageFeedbackConst
{
    public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

    public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

    public static int MaxUrlLength { get; set; } = 256;
    
    public static int MaxUserNoteLength { get; set; } = 1024;
    
    public static int MaxAdminNoteLength { get; set; } = 1024;
    
    public static int MaxEmailAddressesLength { get; set; } = 1024;
    
    public const string EmailAddressesSeparator = ",";
    
    public const string DefaultSettingEntityType = null;
}