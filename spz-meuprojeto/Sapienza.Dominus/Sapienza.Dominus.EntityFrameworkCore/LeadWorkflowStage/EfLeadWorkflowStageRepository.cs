using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadWorkflowStage;

public class EfLeadWorkflowStageRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid>, 
      ILeadWorkflowStageRepository
{
    public EfLeadWorkflowStageRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
