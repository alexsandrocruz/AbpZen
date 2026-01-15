using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadWorkflowStage;

public class EfLeadWorkflowStageRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid>, 
      ILeadWorkflowStageRepository
{
    public EfLeadWorkflowStageRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
