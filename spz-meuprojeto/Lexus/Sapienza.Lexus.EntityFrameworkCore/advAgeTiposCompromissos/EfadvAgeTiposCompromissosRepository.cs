using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

public class EfadvAgeTiposCompromissosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos, Guid>, 
      IadvAgeTiposCompromissosRepository
{
    public EfadvAgeTiposCompromissosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
