using System;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[Serializable]
public class UpdateCmsKitPageFeedbackSettingDto
{
    public bool IsAutoHandled { get; set; }
    public bool RequireCommentsForNegativeFeedback { get; set; }
}