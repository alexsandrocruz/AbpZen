using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabFormasPagamento;

public class EffabFormasPagamentoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento, Guid>, 
      IfabFormasPagamentoRepository
{
    public EffabFormasPagamentoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
