using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Lawyer;

public class EfLawyerRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Lawyer.Lawyer, Guid>, 
      ILawyerRepository
{
    public EfLawyerRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
