using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadScheduledMessage;

public class EfLeadScheduledMessageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid>, 
      ILeadScheduledMessageRepository
{
    public EfLeadScheduledMessageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
