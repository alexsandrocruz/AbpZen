using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRateios;

public class EffinRateiosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finRateios.finRateios, Guid>, 
      IfinRateiosRepository
{
    public EffinRateiosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
