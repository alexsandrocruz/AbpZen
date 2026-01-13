using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabFormasRecebimento;

public class EffabFormasRecebimentoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, Guid>, 
      IfabFormasRecebimentoRepository
{
    public EffabFormasRecebimentoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
