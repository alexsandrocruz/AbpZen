using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Site;

public class EfSiteRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Site.Site, Guid>, 
      ISiteRepository
{
    public EfSiteRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
