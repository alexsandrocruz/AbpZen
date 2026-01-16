using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabPermissoesTipos;

public class EffabPermissoesTiposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos, Guid>, 
      IfabPermissoesTiposRepository
{
    public EffabPermissoesTiposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
