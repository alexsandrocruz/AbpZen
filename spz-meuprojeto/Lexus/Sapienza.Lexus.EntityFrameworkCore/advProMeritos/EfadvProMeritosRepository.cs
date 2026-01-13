using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProMeritos;

public class EfadvProMeritosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProMeritos.advProMeritos, Guid>, 
      IadvProMeritosRepository
{
    public EfadvProMeritosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
