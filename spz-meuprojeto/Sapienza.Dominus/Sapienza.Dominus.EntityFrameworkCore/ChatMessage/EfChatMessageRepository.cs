using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ChatMessage;

public class EfChatMessageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ChatMessage.ChatMessage, Guid>, 
      IChatMessageRepository
{
    public EfChatMessageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
