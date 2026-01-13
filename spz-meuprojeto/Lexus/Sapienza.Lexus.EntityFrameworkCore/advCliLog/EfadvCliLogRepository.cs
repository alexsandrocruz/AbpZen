using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliLog;

public class EfadvCliLogRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliLog.advCliLog, Guid>, 
      IadvCliLogRepository
{
    public EfadvCliLogRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
