using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Workflow;

public class EfWorkflowRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Workflow.Workflow, Guid>, 
      IWorkflowRepository
{
    public EfWorkflowRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
