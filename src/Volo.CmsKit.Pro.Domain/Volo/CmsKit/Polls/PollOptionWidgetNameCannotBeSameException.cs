using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Polls;

[Serializable]
public class PollOptionWidgetNameCannotBeSameException : BusinessException
{
    public PollOptionWidgetNameCannotBeSameException([NotNull] string name)
    : base(CmsKitProErrorCodes.Poll.PollOptionWidgetNameCannotBeSame)
    {
        WithData(nameof(Poll.Widget), name);
    }
}
