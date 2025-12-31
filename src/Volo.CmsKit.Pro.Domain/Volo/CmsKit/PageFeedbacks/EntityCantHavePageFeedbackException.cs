using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.PageFeedbacks;

public class EntityCantHavePageFeedbackException : BusinessException
{
    public string EntityType => Data[nameof(EntityType)] as string;
    
    public EntityCantHavePageFeedbackException([NotNull] string entityType)
        : base(code: CmsKitProErrorCodes.PageFeedbacks.EntityCantHavePageFeedback)
    {
        Check.NotNullOrEmpty(entityType, nameof(entityType), PageFeedbackConst.MaxEntityTypeLength);
        WithData(nameof(EntityType), EntityType);
    }
}