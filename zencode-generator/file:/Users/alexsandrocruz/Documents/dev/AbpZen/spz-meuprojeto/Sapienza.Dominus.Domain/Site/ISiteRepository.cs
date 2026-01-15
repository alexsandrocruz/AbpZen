using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Site;

public interface ISiteRepository : IRepository<Sapienza.Dominus.Site.Site, Guid>
{
}
