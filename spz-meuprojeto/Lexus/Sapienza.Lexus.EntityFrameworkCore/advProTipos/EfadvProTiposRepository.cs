using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProTipos;

public class EfadvProTiposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProTipos.advProTipos, Guid>, 
      IadvProTiposRepository
{
    public EfadvProTiposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
