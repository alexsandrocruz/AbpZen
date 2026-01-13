using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwConfig;

public class EfflwConfigRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.flwConfig.flwConfig, Guid>, 
      IflwConfigRepository
{
    public EfflwConfigRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
