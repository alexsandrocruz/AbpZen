using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProRelevancias;

public class EfadvProRelevanciasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProRelevancias.advProRelevancias, Guid>, 
      IadvProRelevanciasRepository
{
    public EfadvProRelevanciasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
