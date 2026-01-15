using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TimeEntry;

public class EfTimeEntryRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.TimeEntry.TimeEntry, Guid>, 
      ITimeEntryRepository
{
    public EfTimeEntryRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
