using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.AlunoTurma;

public class EfAlunoTurmaRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.AlunoTurma.AlunoTurma, Guid>, 
      IAlunoTurmaRepository
{
    public EfAlunoTurmaRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
