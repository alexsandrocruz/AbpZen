using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosAlteracoes;

public class EfadvProcessosAlteracoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, Guid>, 
      IadvProcessosAlteracoesRepository
{
    public EfadvProcessosAlteracoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
