using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Specialization;

public class EfSpecializationRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Specialization.Specialization, Guid>, 
      ISpecializationRepository
{
    public EfSpecializationRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
