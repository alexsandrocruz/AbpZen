using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProEscritorios;

public class EfadvProEscritoriosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProEscritorios.advProEscritorios, Guid>, 
      IadvProEscritoriosRepository
{
    public EfadvProEscritoriosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
