using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finCentrosCusto;

public class EffinCentrosCustoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finCentrosCusto.finCentrosCusto, Guid>, 
      IfinCentrosCustoRepository
{
    public EffinCentrosCustoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
