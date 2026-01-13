using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advVerTipos;

public class EfadvVerTiposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advVerTipos.advVerTipos, Guid>, 
      IadvVerTiposRepository
{
    public EfadvVerTiposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
