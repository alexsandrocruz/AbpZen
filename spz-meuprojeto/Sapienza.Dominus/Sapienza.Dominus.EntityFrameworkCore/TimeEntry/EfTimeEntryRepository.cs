using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TimeEntry;

public class EfTimeEntryRepository 
    : EfCoreRepository<DominusDbContext, Dominus.TimeEntry.TimeEntry, Guid>, 
      ITimeEntryRepository
{
    public EfTimeEntryRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
