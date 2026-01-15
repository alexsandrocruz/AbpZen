using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.AiChatMessage;

public class EfAiChatMessageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.AiChatMessage.AiChatMessage, Guid>, 
      IAiChatMessageRepository
{
    public EfAiChatMessageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
