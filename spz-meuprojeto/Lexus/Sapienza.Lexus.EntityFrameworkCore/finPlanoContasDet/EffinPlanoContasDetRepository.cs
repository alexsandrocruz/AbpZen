using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPlanoContasDet;

public class EffinPlanoContasDetRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, Guid>, 
      IfinPlanoContasDetRepository
{
    public EffinPlanoContasDetRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
