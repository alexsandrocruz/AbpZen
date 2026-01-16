using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosClientes;

public class EfadvProcessosClientesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessosClientes.advProcessosClientes, Guid>, 
      IadvProcessosClientesRepository
{
    public EfadvProcessosClientesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
