using System;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

[Serializable]
public class FaqSectionInvalidGroupNameException : BusinessException
{
    public FaqSectionInvalidGroupNameException(string groupName)
      : base(CmsKitProErrorCodes.FaqSection.FaqSectionInvalidGroupName)
    {
        WithData(nameof(FaqSection.GroupName), groupName);
    }
}
