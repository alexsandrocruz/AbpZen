using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProInstancias;

public class EfadvProInstanciasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProInstancias.advProInstancias, Guid>, 
      IadvProInstanciasRepository
{
    public EfadvProInstanciasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
