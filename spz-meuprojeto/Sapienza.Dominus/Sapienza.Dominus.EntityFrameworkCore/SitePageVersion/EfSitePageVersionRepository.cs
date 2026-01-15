using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SitePageVersion;

public class EfSitePageVersionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.SitePageVersion.SitePageVersion, Guid>, 
      ISitePageVersionRepository
{
    public EfSitePageVersionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
