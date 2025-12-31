using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Polls;
public interface IPollUserVoteRepository : IBasicRepository<PollUserVote, Guid>
{
}
