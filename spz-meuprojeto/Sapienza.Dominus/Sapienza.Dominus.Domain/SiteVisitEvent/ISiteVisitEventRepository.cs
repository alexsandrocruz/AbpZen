using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SiteVisitEvent;

public interface ISiteVisitEventRepository : IRepository<Dominus.SiteVisitEvent.SiteVisitEvent, Guid>
{
}
