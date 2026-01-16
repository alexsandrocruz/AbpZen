using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPlanoContasGrupos;

public class EffinPlanoContasGruposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, Guid>, 
      IfinPlanoContasGruposRepository
{
    public EffinPlanoContasGruposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
