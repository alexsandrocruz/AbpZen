using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPlanoContasDet;

public class EffinPlanoContasDetRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, Guid>, 
      IfinPlanoContasDetRepository
{
    public EffinPlanoContasDetRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
