using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Workflow;

public interface IWorkflowRepository : IRepository<Dominus.Workflow.Workflow, Guid>
{
}
