using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finLancamentos_BKP;

public class EffinLancamentos_BKPRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finLancamentos_BKP.finLancamentos_BKP, Guid>, 
      IfinLancamentos_BKPRepository
{
    public EffinLancamentos_BKPRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
