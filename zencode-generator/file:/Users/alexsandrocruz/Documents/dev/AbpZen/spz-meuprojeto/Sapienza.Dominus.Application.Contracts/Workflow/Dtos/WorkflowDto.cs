using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Workflow.Dtos;

[Serializable]
public class WorkflowDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string TriggerType { get; set; }
    public string Status { get; set; }
    public string Actions { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
