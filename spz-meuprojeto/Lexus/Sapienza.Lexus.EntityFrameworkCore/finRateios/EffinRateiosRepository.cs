using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRateios;

public class EffinRateiosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finRateios.finRateios, Guid>, 
      IfinRateiosRepository
{
    public EffinRateiosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
