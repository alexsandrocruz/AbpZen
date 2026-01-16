using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabCidades;

public class EffabCidadesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabCidades.fabCidades, Guid>, 
      IfabCidadesRepository
{
    public EffabCidadesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
