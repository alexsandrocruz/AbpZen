using Volo.Abp;

namespace Volo.CmsKit.PageFeedbacks;

public class RequireCommentsForNegativeFeedbackException : BusinessException
{
    public RequireCommentsForNegativeFeedbackException() : base(CmsKitProErrorCodes.PageFeedbacks.RequireCommentsForNegativeFeedback)
    {
    }
}