using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Conversation;

public class EfConversationRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Conversation.Conversation, Guid>, 
      IConversationRepository
{
    public EfConversationRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
