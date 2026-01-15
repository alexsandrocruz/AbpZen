using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SiteVisitDailyStat;

public interface ISiteVisitDailyStatRepository : IRepository<Sapienza.Dominus.SiteVisitDailyStat.SiteVisitDailyStat, Guid>
{
}
