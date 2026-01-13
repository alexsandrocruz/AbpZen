using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabMotivosAproveitamento;

public class EffabMotivosAproveitamentoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabMotivosAproveitamento.fabMotivosAproveitamento, Guid>, 
      IfabMotivosAproveitamentoRepository
{
    public EffabMotivosAproveitamentoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
