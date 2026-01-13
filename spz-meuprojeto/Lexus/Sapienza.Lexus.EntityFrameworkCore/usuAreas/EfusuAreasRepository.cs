using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuAreas;

public class EfusuAreasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.usuAreas.usuAreas, Guid>, 
      IusuAreasRepository
{
    public EfusuAreasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
