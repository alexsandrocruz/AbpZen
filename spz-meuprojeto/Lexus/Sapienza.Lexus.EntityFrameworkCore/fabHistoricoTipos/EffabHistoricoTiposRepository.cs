using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabHistoricoTipos;

public class EffabHistoricoTiposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, Guid>, 
      IfabHistoricoTiposRepository
{
    public EffabHistoricoTiposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
