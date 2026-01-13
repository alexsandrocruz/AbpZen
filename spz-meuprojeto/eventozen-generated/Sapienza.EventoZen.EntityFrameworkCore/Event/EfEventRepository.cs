using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.Event;

public class EfEventRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.Event.Event, Guid>, 
      IEventRepository
{
    public EfEventRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
