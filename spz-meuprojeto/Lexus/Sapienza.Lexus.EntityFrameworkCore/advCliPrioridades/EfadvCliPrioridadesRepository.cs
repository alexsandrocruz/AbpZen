using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliPrioridades;

public class EfadvCliPrioridadesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliPrioridades.advCliPrioridades, Guid>, 
      IadvCliPrioridadesRepository
{
    public EfadvCliPrioridadesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
