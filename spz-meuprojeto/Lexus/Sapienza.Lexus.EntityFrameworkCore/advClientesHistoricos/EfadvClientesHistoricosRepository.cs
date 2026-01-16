using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesHistoricos;

public class EfadvClientesHistoricosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid>, 
      IadvClientesHistoricosRepository
{
    public EfadvClientesHistoricosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
