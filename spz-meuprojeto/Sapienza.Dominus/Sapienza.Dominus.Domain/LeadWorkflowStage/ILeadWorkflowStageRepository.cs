using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadWorkflowStage;

public interface ILeadWorkflowStageRepository : IRepository<Dominus.LeadWorkflowStage.LeadWorkflowStage, Guid>
{
}
