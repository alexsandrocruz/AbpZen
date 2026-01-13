using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finContasClientes;

public class EffinContasClientesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finContasClientes.finContasClientes, Guid>, 
      IfinContasClientesRepository
{
    public EffinContasClientesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
