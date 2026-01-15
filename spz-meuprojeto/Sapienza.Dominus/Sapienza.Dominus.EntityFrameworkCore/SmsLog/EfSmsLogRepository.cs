using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SmsLog;

public class EfSmsLogRepository 
    : EfCoreRepository<DominusDbContext, Dominus.SmsLog.SmsLog, Guid>, 
      ISmsLogRepository
{
    public EfSmsLogRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
