using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProjectFollower;

public class EfProjectFollowerRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ProjectFollower.ProjectFollower, Guid>, 
      IProjectFollowerRepository
{
    public EfProjectFollowerRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
