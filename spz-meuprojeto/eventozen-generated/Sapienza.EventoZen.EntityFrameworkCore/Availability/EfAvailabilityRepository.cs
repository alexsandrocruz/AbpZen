using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.Availability;

public class EfAvailabilityRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.Availability.Availability, Guid>, 
      IAvailabilityRepository
{
    public EfAvailabilityRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
