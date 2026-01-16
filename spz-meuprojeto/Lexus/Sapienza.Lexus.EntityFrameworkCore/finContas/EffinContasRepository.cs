using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finContas;

public class EffinContasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finContas.finContas, Guid>, 
      IfinContasRepository
{
    public EffinContasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
