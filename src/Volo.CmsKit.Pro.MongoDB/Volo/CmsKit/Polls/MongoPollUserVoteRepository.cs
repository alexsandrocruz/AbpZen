using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Volo.CmsKit.Polls;
public class MongoPollUserVoteRepository : MongoDbRepository<ICmsKitProMongoDbContext, PollUserVote, Guid>, IPollUserVoteRepository
{
    public MongoPollUserVoteRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

}
