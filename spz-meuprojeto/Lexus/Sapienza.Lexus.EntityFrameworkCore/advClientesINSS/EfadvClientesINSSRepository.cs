using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesINSS;

public class EfadvClientesINSSRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesINSS.advClientesINSS, Guid>, 
      IadvClientesINSSRepository
{
    public EfadvClientesINSSRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
