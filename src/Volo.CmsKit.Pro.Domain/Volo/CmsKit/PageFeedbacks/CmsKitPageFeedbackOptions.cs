using JetBrains.Annotations;

namespace Volo.CmsKit.PageFeedbacks;

public class CmsKitPageFeedbackOptions
{
    [NotNull] 
    public PageFeedbackEntityTypeDefinitions EntityTypes { get; } = new();
}