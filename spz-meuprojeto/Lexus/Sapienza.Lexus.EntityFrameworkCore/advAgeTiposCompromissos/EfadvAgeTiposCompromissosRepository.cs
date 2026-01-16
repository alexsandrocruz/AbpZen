using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

public class EfadvAgeTiposCompromissosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, Guid>, 
      IadvAgeTiposCompromissosRepository
{
    public EfadvAgeTiposCompromissosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
