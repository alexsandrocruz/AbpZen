using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LandingLead;

public class EfLandingLeadRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LandingLead.LandingLead, Guid>, 
      ILandingLeadRepository
{
    public EfLandingLeadRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
