using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ChatMessage;

public class EfChatMessageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ChatMessage.ChatMessage, Guid>, 
      IChatMessageRepository
{
    public EfChatMessageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
