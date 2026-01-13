using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliCargos;

public class EfadvCliCargosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliCargos.advCliCargos, Guid>, 
      IadvCliCargosRepository
{
    public EfadvCliCargosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
