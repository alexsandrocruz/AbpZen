using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProNaturezas;

public class EfadvProNaturezasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProNaturezas.advProNaturezas, Guid>, 
      IadvProNaturezasRepository
{
    public EfadvProNaturezasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
