using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkflowExecution;

public class EfWorkflowExecutionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.WorkflowExecution.WorkflowExecution, Guid>, 
      IWorkflowExecutionRepository
{
    public EfWorkflowExecutionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
