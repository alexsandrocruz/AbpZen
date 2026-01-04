using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Aluno;

public class EfAlunoRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.Aluno.Aluno, Guid>, 
      IAlunoRepository
{
    public EfAlunoRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
