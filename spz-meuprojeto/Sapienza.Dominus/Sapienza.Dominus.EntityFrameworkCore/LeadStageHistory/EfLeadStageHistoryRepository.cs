using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadStageHistory;

public class EfLeadStageHistoryRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadStageHistory.LeadStageHistory, Guid>, 
      ILeadStageHistoryRepository
{
    public EfLeadStageHistoryRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
