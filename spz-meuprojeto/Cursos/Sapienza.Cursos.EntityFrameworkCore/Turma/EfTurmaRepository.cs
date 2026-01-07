using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Turma;

public class EfTurmaRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Turma.Turma, Guid>, 
      ITurmaRepository
{
    public EfTurmaRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
