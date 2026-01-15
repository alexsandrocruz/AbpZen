using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadLandingPage;

public class EfLeadLandingPageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadLandingPage.LeadLandingPage, Guid>, 
      ILeadLandingPageRepository
{
    public EfLeadLandingPageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
