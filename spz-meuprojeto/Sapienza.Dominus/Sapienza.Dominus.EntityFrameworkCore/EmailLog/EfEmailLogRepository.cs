using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.EmailLog;

public class EfEmailLogRepository 
    : EfCoreRepository<DominusDbContext, Dominus.EmailLog.EmailLog, Guid>, 
      IEmailLogRepository
{
    public EfEmailLogRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
