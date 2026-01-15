using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkflowExecution;

public class EfWorkflowExecutionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.WorkflowExecution.WorkflowExecution, Guid>, 
      IWorkflowExecutionRepository
{
    public EfWorkflowExecutionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
