using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.AiChatMessage;

public class EfAiChatMessageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.AiChatMessage.AiChatMessage, Guid>, 
      IAiChatMessageRepository
{
    public EfAiChatMessageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
