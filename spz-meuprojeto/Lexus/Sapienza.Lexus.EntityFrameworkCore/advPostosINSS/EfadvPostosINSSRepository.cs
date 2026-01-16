using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPostosINSS;

public class EfadvPostosINSSRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPostosINSS.advPostosINSS, Guid>, 
      IadvPostosINSSRepository
{
    public EfadvPostosINSSRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
