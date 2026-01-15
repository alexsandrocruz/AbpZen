using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SiteVisitDailyStat;

public interface ISiteVisitDailyStatRepository : IRepository<Dominus.SiteVisitDailyStat.SiteVisitDailyStat, Guid>
{
}
