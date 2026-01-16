using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreMetas;

public class EfadvPreMetasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreMetas.advPreMetas, Guid>, 
      IadvPreMetasRepository
{
    public EfadvPreMetasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
