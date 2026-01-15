using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ClientMessage;

public class EfClientMessageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ClientMessage.ClientMessage, Guid>, 
      IClientMessageRepository
{
    public EfClientMessageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
