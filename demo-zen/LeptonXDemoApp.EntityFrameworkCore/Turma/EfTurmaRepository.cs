using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Turma;

public class EfTurmaRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.Turma.Turma, Guid>, 
      ITurmaRepository
{
    public EfTurmaRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
