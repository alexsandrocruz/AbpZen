using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.AiChatSession;

public class EfAiChatSessionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.AiChatSession.AiChatSession, Guid>, 
      IAiChatSessionRepository
{
    public EfAiChatSessionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
