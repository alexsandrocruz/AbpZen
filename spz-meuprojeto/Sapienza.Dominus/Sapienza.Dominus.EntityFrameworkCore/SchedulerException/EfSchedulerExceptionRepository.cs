using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SchedulerException;

public class EfSchedulerExceptionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.SchedulerException.SchedulerException, Guid>, 
      ISchedulerExceptionRepository
{
    public EfSchedulerExceptionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
