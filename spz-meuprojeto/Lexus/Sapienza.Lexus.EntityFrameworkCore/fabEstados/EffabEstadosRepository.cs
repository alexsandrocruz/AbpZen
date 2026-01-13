using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabEstados;

public class EffabEstadosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabEstados.fabEstados, Guid>, 
      IfabEstadosRepository
{
    public EffabEstadosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
