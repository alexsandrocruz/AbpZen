using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkflowExecution.Dtos;

[Serializable]
public class WorkflowExecutionDto : FullAuditedEntityDto<Guid>
{
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? WorkflowId { get; set; }
    public string? WorkflowDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
