using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadAutomation;

public class EfLeadAutomationRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadAutomation.LeadAutomation, Guid>, 
      ILeadAutomationRepository
{
    public EfLeadAutomationRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
