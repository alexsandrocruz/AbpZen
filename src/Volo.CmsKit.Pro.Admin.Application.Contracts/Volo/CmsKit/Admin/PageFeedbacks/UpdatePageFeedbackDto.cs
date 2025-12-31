using System;
using Volo.Abp.Validation;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[Serializable]
public class UpdatePageFeedbackDto
{
    public bool IsHandled { get; set; }
    
    [DynamicMaxLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxAdminNoteLength))]
    public string AdminNote { get; set; }
}