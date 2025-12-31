using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Polls;
public class EfCorePollUserVoteRepository : EfCoreRepository<ICmsKitProDbContext, PollUserVote, Guid>, IPollUserVoteRepository
{
    public EfCorePollUserVoteRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
