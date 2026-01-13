using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesHistoricos;

public class EfadvClientesHistoricosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid>, 
      IadvClientesHistoricosRepository
{
    public EfadvClientesHistoricosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
