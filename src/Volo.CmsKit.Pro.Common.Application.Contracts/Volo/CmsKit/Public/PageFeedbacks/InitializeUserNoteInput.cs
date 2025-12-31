using System;
using Volo.Abp.Validation;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Public.PageFeedbacks;

[Serializable]
public class InitializeUserNoteInput
{
    public Guid Id { get; set; }
    
    [DynamicStringLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxUserNoteLength))]
    public string UserNote { get; set; }
    
    public bool IsUseful { get; set; }
}