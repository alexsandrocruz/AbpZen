using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreMetas;

public class EfadvPreMetasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreMetas.advPreMetas, Guid>, 
      IadvPreMetasRepository
{
    public EfadvPreMetasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
