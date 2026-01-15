using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SitePageVersion;

public class EfSitePageVersionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SitePageVersion.SitePageVersion, Guid>, 
      ISitePageVersionRepository
{
    public EfSitePageVersionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
