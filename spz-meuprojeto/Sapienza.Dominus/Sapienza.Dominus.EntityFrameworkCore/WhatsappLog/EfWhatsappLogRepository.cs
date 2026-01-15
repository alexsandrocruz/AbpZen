using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WhatsappLog;

public class EfWhatsappLogRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.WhatsappLog.WhatsappLog, Guid>, 
      IWhatsappLogRepository
{
    public EfWhatsappLogRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
