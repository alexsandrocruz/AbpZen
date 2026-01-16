using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCompromissos;

public class EfadvCompromissosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCompromissos.advCompromissos, Guid>, 
      IadvCompromissosRepository
{
    public EfadvCompromissosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
