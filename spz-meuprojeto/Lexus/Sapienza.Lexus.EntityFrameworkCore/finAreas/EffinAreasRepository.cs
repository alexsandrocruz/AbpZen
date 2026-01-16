using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finAreas;

public class EffinAreasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finAreas.finAreas, Guid>, 
      IfinAreasRepository
{
    public EffinAreasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
