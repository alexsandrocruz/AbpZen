using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advVerTipos;

public class EfadvVerTiposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advVerTipos.advVerTipos, Guid>, 
      IadvVerTiposRepository
{
    public EfadvVerTiposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
