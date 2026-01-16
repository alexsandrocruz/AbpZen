using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advVerbas;

public class EfadvVerbasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advVerbas.advVerbas, Guid>, 
      IadvVerbasRepository
{
    public EfadvVerbasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
