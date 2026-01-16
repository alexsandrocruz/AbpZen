using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabFormasRecebimento;

public class EffabFormasRecebimentoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, Guid>, 
      IfabFormasRecebimentoRepository
{
    public EffabFormasRecebimentoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
