using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuDistancias;

public class EfusuDistanciasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.usuDistancias.usuDistancias, Guid>, 
      IusuDistanciasRepository
{
    public EfusuDistanciasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
