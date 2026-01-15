using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadWorkflowStage.Dtos;

[Serializable]
public class LeadWorkflowStageDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public int Position { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public string? LeadWorkflowDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
