using System;

namespace Volo.CmsKit.Public.PageFeedbacks;

[Serializable]
public class ChangeIsUsefulInput
{
    public Guid Id { get; set; }
    
    public bool IsUseful { get; set; }
}