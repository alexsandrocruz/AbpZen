using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SitePageVersion;

public interface ISitePageVersionRepository : IRepository<Dominus.SitePageVersion.SitePageVersion, Guid>
{
}
