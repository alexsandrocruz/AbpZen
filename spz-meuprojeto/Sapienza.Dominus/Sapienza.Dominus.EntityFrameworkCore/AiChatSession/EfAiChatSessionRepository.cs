using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.AiChatSession;

public class EfAiChatSessionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.AiChatSession.AiChatSession, Guid>, 
      IAiChatSessionRepository
{
    public EfAiChatSessionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
