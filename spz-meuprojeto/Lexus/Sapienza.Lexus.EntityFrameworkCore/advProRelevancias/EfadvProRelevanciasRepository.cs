using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProRelevancias;

public class EfadvProRelevanciasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProRelevancias.advProRelevancias, Guid>, 
      IadvProRelevanciasRepository
{
    public EfadvProRelevanciasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
