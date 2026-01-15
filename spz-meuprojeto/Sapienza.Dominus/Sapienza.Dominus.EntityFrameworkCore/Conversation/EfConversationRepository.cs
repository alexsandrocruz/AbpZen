using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Conversation;

public class EfConversationRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Conversation.Conversation, Guid>, 
      IConversationRepository
{
    public EfConversationRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
