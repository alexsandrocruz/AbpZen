using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabPaises;

public class EffabPaisesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabPaises.fabPaises, Guid>, 
      IfabPaisesRepository
{
    public EffabPaisesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
