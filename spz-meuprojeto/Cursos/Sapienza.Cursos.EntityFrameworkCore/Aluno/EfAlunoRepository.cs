using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Aluno;

public class EfAlunoRepository 
    : EfCoreRepository<CursosDbContext, Sapienza.Cursos.Aluno.Aluno, Guid>, 
      IAlunoRepository
{
    public EfAlunoRepository(IDbContextProvider<CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
