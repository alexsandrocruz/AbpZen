using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkflowExecution;

public interface IWorkflowExecutionRepository : IRepository<Sapienza.Dominus.WorkflowExecution.WorkflowExecution, Guid>
{
}
