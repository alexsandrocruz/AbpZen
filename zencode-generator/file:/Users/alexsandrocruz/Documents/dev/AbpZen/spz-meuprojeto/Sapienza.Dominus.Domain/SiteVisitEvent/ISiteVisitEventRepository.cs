using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SiteVisitEvent;

public interface ISiteVisitEventRepository : IRepository<Sapienza.Dominus.SiteVisitEvent.SiteVisitEvent, Guid>
{
}
