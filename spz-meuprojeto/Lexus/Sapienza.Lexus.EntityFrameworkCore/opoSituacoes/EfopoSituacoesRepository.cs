using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoSituacoes;

public class EfopoSituacoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.opoSituacoes.opoSituacoes, Guid>, 
      IopoSituacoesRepository
{
    public EfopoSituacoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
