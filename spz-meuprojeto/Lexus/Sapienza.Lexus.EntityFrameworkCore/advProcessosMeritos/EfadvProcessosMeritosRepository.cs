using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosMeritos;

public class EfadvProcessosMeritosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos, Guid>, 
      IadvProcessosMeritosRepository
{
    public EfadvProcessosMeritosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
