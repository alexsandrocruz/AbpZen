using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Site;

public class EfSiteRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Site.Site, Guid>, 
      ISiteRepository
{
    public EfSiteRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
