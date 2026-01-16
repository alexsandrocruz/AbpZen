using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProTipos;

public class EfadvProTiposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProTipos.advProTipos, Guid>, 
      IadvProTiposRepository
{
    public EfadvProTiposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
