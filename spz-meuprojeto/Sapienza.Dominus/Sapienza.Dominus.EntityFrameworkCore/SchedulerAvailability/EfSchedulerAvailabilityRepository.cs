using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SchedulerAvailability;

public class EfSchedulerAvailabilityRepository 
    : EfCoreRepository<DominusDbContext, Dominus.SchedulerAvailability.SchedulerAvailability, Guid>, 
      ISchedulerAvailabilityRepository
{
    public EfSchedulerAvailabilityRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
