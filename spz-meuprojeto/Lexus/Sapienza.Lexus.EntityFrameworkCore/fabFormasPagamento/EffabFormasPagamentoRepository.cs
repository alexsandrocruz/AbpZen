using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabFormasPagamento;

public class EffabFormasPagamentoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, Guid>, 
      IfabFormasPagamentoRepository
{
    public EffabFormasPagamentoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
