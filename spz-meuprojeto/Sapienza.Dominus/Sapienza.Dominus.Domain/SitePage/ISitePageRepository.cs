using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SitePage;

public interface ISitePageRepository : IRepository<Dominus.SitePage.SitePage, Guid>
{
}
