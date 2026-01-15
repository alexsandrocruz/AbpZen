using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadScheduledMessage;

public class EfLeadScheduledMessageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid>, 
      ILeadScheduledMessageRepository
{
    public EfLeadScheduledMessageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
