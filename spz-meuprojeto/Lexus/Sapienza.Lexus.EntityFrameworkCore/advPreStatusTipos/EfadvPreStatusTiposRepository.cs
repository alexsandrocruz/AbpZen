using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreStatusTipos;

public class EfadvPreStatusTiposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos, Guid>, 
      IadvPreStatusTiposRepository
{
    public EfadvPreStatusTiposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
