using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProfissionaisNaturezas;

public class EfadvProfissionaisNaturezasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, Guid>, 
      IadvProfissionaisNaturezasRepository
{
    public EfadvProfissionaisNaturezasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
