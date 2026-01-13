using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabDatasEFeriados;

public class EffabDatasEFeriadosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados, Guid>, 
      IfabDatasEFeriadosRepository
{
    public EffabDatasEFeriadosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
