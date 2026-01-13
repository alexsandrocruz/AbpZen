using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPlanoContasGrupos;

public class EffinPlanoContasGruposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, Guid>, 
      IfinPlanoContasGruposRepository
{
    public EffinPlanoContasGruposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
