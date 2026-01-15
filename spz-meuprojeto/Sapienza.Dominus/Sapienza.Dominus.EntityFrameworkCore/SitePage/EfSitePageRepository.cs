using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SitePage;

public class EfSitePageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SitePage.SitePage, Guid>, 
      ISitePageRepository
{
    public EfSitePageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
