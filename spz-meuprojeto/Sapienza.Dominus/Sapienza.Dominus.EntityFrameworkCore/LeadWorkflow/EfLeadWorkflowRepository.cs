using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadWorkflow;

public class EfLeadWorkflowRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadWorkflow.LeadWorkflow, Guid>, 
      ILeadWorkflowRepository
{
    public EfLeadWorkflowRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
