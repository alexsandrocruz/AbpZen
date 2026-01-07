using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.NewEntity1;

public class EfNewEntity1Repository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.NewEntity1.NewEntity1, Guid>, 
      INewEntity1Repository
{
    public EfNewEntity1Repository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
