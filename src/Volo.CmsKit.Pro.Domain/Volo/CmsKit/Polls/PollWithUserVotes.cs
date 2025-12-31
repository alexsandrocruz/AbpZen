using System.Collections.Generic;

namespace Volo.CmsKit.Polls;

public class PollWithUserVotes
{
    public Poll Poll { get; set; }

    public IEnumerable<PollUserVote> UserVotes { get; set; }
}