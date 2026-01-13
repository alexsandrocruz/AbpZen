using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finUnidades;

public class EffinUnidadesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finUnidades.finUnidades, Guid>, 
      IfinUnidadesRepository
{
    public EffinUnidadesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
