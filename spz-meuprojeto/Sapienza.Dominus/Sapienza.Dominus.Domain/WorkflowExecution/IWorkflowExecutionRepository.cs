using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkflowExecution;

public interface IWorkflowExecutionRepository : IRepository<Dominus.WorkflowExecution.WorkflowExecution, Guid>
{
}
