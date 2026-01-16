using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliGrupos;

public class EfadvCliGruposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliGrupos.advCliGrupos, Guid>, 
      IadvCliGruposRepository
{
    public EfadvCliGruposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
