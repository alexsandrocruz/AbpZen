using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finAreas;

public class EffinAreasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finAreas.finAreas, Guid>, 
      IfinAreasRepository
{
    public EffinAreasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
