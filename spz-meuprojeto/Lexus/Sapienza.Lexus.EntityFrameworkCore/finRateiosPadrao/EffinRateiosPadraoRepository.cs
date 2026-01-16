using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRateiosPadrao;

public class EffinRateiosPadraoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, Guid>, 
      IfinRateiosPadraoRepository
{
    public EffinRateiosPadraoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
