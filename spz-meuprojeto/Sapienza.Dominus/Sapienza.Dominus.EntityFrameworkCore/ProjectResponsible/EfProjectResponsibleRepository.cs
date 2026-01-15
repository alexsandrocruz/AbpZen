using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProjectResponsible;

public class EfProjectResponsibleRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ProjectResponsible.ProjectResponsible, Guid>, 
      IProjectResponsibleRepository
{
    public EfProjectResponsibleRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
