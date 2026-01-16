using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientes;

public class EfadvClientesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientes.advClientes, Guid>, 
      IadvClientesRepository
{
    public EfadvClientesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
