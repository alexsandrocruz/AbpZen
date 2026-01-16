using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliBairros;

public class EfadvCliBairrosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliBairros.advCliBairros, Guid>, 
      IadvCliBairrosRepository
{
    public EfadvCliBairrosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
