using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProFases;

public class EfadvProFasesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProFases.advProFases, Guid>, 
      IadvProFasesRepository
{
    public EfadvProFasesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
