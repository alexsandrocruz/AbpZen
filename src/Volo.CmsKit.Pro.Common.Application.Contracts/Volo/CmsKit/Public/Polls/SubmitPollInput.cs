using System;

namespace Volo.CmsKit.Public.Polls;

[Serializable]
public class SubmitPollInput
{
    public Guid[] PollOptionIds { get; set; }
}
