using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Project;

public class EfProjectRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Project.Project, Guid>, 
      IProjectRepository
{
    public EfProjectRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
