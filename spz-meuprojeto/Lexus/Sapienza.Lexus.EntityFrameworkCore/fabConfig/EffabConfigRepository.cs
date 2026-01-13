using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabConfig;

public class EffabConfigRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabConfig.fabConfig, Guid>, 
      IfabConfigRepository
{
    public EffabConfigRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
