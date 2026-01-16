using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabConfig;

public class EffabConfigRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabConfig.fabConfig, Guid>, 
      IfabConfigRepository
{
    public EffabConfigRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
