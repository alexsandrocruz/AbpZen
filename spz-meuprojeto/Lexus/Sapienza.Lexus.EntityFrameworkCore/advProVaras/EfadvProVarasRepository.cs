using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProVaras;

public class EfadvProVarasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProVaras.advProVaras, Guid>, 
      IadvProVarasRepository
{
    public EfadvProVarasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
