using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LandingLead;

public class EfLandingLeadRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LandingLead.LandingLead, Guid>, 
      ILandingLeadRepository
{
    public EfLandingLeadRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
