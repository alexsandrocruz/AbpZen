using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPautaObs;

public class EfadvPautaObsRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPautaObs.advPautaObs, Guid>, 
      IadvPautaObsRepository
{
    public EfadvPautaObsRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
