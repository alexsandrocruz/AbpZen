using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabCondicoesPagamento;

public class EffabCondicoesPagamentoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento, Guid>, 
      IfabCondicoesPagamentoRepository
{
    public EffabCondicoesPagamentoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
