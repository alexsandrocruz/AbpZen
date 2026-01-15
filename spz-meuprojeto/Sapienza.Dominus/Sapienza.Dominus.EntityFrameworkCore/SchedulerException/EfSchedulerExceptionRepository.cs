using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SchedulerException;

public class EfSchedulerExceptionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SchedulerException.SchedulerException, Guid>, 
      ISchedulerExceptionRepository
{
    public EfSchedulerExceptionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
