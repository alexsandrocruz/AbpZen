using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Workflow;

public class EfWorkflowRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Workflow.Workflow, Guid>, 
      IWorkflowRepository
{
    public EfWorkflowRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
