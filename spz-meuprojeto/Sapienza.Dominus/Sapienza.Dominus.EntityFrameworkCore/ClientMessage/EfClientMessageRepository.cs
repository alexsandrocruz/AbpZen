using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ClientMessage;

public class EfClientMessageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ClientMessage.ClientMessage, Guid>, 
      IClientMessageRepository
{
    public EfClientMessageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
