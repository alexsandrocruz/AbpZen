using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProjectFollower;

public class EfProjectFollowerRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ProjectFollower.ProjectFollower, Guid>, 
      IProjectFollowerRepository
{
    public EfProjectFollowerRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
