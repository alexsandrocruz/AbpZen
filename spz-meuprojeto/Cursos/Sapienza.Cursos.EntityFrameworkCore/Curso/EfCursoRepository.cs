using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Curso;

public class EfCursoRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Curso.Curso, Guid>, 
      ICursoRepository
{
    public EfCursoRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
