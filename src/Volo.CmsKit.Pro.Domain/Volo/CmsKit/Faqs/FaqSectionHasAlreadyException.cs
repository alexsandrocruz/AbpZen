using System;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

[Serializable]
public class FaqSectionHasAlreadyException : BusinessException
{
    public FaqSectionHasAlreadyException(string groupName, string name)
        : base(CmsKitProErrorCodes.FaqSection.FaqSectionHasAlreadyException)
    {
        WithData(nameof(FaqSection.GroupName), groupName);
        WithData(nameof(FaqSection.Name), name);
    }
}

