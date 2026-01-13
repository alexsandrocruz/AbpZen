using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.Client;

public class EfClientRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.Client.Client, Guid>, 
      IClientRepository
{
    public EfClientRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
