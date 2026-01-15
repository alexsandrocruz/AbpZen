using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.SmsLog;

public class EfSmsLogRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.SmsLog.SmsLog, Guid>, 
      ISmsLogRepository
{
    public EfSmsLogRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
