using System;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

[Serializable]
public class FaqQuestionHasAlreadyException : BusinessException
{
    public FaqQuestionHasAlreadyException(string title)
        : base(CmsKitProErrorCodes.FaqQuestion.FaqQuestionHasAlreadyException)
    {
        WithData(nameof(FaqQuestion.Title), title);
    }
}