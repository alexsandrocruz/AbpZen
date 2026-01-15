using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SiteVisitEvent;

public class EfSiteVisitEventRepository 
    : EfCoreRepository<DominusDbContext, Dominus.SiteVisitEvent.SiteVisitEvent, Guid>, 
      ISiteVisitEventRepository
{
    public EfSiteVisitEventRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
