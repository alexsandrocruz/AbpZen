using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SiteVisitEvent;

public class EfSiteVisitEventRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SiteVisitEvent.SiteVisitEvent, Guid>, 
      ISiteVisitEventRepository
{
    public EfSiteVisitEventRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
