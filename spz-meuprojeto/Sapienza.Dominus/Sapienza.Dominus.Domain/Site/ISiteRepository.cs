using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Site;

public interface ISiteRepository : IRepository<Dominus.Site.Site, Guid>
{
}
