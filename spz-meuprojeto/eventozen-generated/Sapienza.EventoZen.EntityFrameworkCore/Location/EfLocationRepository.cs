using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.Location;

public class EfLocationRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.Location.Location, Guid>, 
      ILocationRepository
{
    public EfLocationRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
