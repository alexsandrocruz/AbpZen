using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientes_bkp;

public class EfadvClientes_bkpRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advClientes_bkp.advClientes_bkp, Guid>, 
      IadvClientes_bkpRepository
{
    public EfadvClientes_bkpRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
