using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProVaras;

public class EfadvProVarasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProVaras.advProVaras, Guid>, 
      IadvProVarasRepository
{
    public EfadvProVarasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
