using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabDatasEFeriados;

public class EffabDatasEFeriadosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, Guid>, 
      IfabDatasEFeriadosRepository
{
    public EffabDatasEFeriadosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
