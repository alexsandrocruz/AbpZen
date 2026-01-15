using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SiteVisitDailyStat;

public class EfSiteVisitDailyStatRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SiteVisitDailyStat.SiteVisitDailyStat, Guid>, 
      ISiteVisitDailyStatRepository
{
    public EfSiteVisitDailyStatRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
