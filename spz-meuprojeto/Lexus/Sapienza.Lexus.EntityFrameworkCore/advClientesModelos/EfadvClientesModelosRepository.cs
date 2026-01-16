using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesModelos;

public class EfadvClientesModelosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesModelos.advClientesModelos, Guid>, 
      IadvClientesModelosRepository
{
    public EfadvClientesModelosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
