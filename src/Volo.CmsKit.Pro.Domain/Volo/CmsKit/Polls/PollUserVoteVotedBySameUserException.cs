using System;
using Volo.Abp;

namespace Volo.CmsKit.Polls;

[Serializable]
public class PollUserVoteVotedBySameUserException : BusinessException
{
    public PollUserVoteVotedBySameUserException(Guid userId, Guid pollId)
    : base(CmsKitProErrorCodes.Poll.PollUserVoteVotedBySameUser)
    {
        WithData(nameof(PollUserVote.UserId), userId);
        WithData(nameof(PollUserVote.PollId), pollId);
    }
}
