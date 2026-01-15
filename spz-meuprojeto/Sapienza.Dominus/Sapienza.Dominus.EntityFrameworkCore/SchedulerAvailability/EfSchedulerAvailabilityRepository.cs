using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SchedulerAvailability;

public class EfSchedulerAvailabilityRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SchedulerAvailability.SchedulerAvailability, Guid>, 
      ISchedulerAvailabilityRepository
{
    public EfSchedulerAvailabilityRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
