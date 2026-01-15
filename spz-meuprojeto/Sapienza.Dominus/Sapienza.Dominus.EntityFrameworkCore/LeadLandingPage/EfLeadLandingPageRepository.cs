using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadLandingPage;

public class EfLeadLandingPageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadLandingPage.LeadLandingPage, Guid>, 
      ILeadLandingPageRepository
{
    public EfLeadLandingPageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
