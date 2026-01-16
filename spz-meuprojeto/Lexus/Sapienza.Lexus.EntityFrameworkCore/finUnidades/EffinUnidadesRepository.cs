using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finUnidades;

public class EffinUnidadesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finUnidades.finUnidades, Guid>, 
      IfinUnidadesRepository
{
    public EffinUnidadesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
