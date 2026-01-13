using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliBairros;

public class EfadvCliBairrosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliBairros.advCliBairros, Guid>, 
      IadvCliBairrosRepository
{
    public EfadvCliBairrosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
