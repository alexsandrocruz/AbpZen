using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliLog;

public class EfadvCliLogRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliLog.advCliLog, Guid>, 
      IadvCliLogRepository
{
    public EfadvCliLogRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
