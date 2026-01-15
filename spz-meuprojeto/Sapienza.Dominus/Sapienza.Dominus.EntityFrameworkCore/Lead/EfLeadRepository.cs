using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Lead;

public class EfLeadRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Lead.Lead, Guid>, 
      ILeadRepository
{
    public EfLeadRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
