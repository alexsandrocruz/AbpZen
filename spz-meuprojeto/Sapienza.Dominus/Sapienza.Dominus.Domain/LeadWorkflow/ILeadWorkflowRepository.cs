using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadWorkflow;

public interface ILeadWorkflowRepository : IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid>
{
}
