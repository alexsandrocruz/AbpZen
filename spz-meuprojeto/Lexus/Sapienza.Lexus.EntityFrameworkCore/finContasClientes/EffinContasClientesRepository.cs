using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finContasClientes;

public class EffinContasClientesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finContasClientes.finContasClientes, Guid>, 
      IfinContasClientesRepository
{
    public EffinContasClientesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
