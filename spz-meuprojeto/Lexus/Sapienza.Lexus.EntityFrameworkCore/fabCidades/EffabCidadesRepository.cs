using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabCidades;

public class EffabCidadesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabCidades.fabCidades, Guid>, 
      IfabCidadesRepository
{
    public EffabCidadesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
