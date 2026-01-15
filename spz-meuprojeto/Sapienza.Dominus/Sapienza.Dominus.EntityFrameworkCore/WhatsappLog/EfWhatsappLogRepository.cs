using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WhatsappLog;

public class EfWhatsappLogRepository 
    : EfCoreRepository<DominusDbContext, Dominus.WhatsappLog.WhatsappLog, Guid>, 
      IWhatsappLogRepository
{
    public EfWhatsappLogRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
