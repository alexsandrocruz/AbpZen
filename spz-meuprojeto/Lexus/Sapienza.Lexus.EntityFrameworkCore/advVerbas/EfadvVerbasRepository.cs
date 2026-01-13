using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advVerbas;

public class EfadvVerbasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advVerbas.advVerbas, Guid>, 
      IadvVerbasRepository
{
    public EfadvVerbasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
