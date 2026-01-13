using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoSituacoes;

public class EfopoSituacoesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.opoSituacoes.opoSituacoes, Guid>, 
      IopoSituacoesRepository
{
    public EfopoSituacoesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
