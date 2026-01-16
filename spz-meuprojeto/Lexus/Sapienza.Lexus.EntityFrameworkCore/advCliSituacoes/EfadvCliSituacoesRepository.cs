using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliSituacoes;

public class EfadvCliSituacoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliSituacoes.advCliSituacoes, Guid>, 
      IadvCliSituacoesRepository
{
    public EfadvCliSituacoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
