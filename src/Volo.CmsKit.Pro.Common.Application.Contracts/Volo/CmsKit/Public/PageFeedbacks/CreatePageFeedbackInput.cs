using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Public.PageFeedbacks;

[Serializable]
public class CreatePageFeedbackInput
{
    [DynamicStringLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxUserNoteLength))]
    public string Url { get; set; }
    
    [Required]
    public bool IsUseful { get; set; }
    
    [DynamicStringLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxUserNoteLength))]
    public string UserNote { get; set; }
    
    [DynamicStringLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxEntityTypeLength))]
    [Required]
    public string EntityType { get; set; }
    
    [DynamicStringLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxEntityIdLength))]
    public string EntityId { get; set; }
    
    public Guid FeedbackUserId { get; set; }
}