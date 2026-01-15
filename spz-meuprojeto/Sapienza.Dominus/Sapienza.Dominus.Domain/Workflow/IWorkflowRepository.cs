using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Workflow;

public interface IWorkflowRepository : IRepository<Sapienza.Dominus.Workflow.Workflow, Guid>
{
}
