using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.EmailLog;

public class EfEmailLogRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.EmailLog.EmailLog, Guid>, 
      IEmailLogRepository
{
    public EfEmailLogRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
