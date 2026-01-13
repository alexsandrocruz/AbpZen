using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRateiosPadrao;

public class EffinRateiosPadraoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, Guid>, 
      IfinRateiosPadraoRepository
{
    public EffinRateiosPadraoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
