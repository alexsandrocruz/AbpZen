using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SchedulerType;

public class EfSchedulerTypeRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SchedulerType.SchedulerType, Guid>, 
      ISchedulerTypeRepository
{
    public EfSchedulerTypeRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
