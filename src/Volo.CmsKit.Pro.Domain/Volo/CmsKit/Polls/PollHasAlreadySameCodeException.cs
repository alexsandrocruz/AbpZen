using System;
using Volo.Abp;

namespace Volo.CmsKit.Polls;

[Serializable]
public class PollHasAlreadySameCodeException : BusinessException
{
    public PollHasAlreadySameCodeException(string code)
    : base(CmsKitProErrorCodes.Poll.PollHasAlreadySameCodeException)
    {
        WithData(nameof(Poll.Code), code);
    }
}
