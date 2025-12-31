using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Polls;

[Serializable]
public class PollResultShowingEndDateCannotSetBeforeStartDateException : BusinessException
{
    public PollResultShowingEndDateCannotSetBeforeStartDateException([NotNull] DateTime startDate, [NotNull] DateTime resultShowingEndDate)
    : base(CmsKitProErrorCodes.Poll.PollResultShowingEndDateCannotSetBeforeStartDate)
    {
        WithData(nameof(Poll.StartDate), startDate);
        WithData(nameof(Poll.ResultShowingEndDate), resultShowingEndDate);
    }
}
