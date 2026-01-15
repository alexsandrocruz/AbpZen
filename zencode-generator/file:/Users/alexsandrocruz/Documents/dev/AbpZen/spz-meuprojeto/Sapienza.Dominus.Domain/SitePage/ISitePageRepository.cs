using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SitePage;

public interface ISitePageRepository : IRepository<Sapienza.Dominus.SitePage.SitePage, Guid>
{
}
