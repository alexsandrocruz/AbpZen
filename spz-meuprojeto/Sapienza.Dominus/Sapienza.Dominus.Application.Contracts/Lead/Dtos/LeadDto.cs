using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Lead.Dtos;

[Serializable]
public class LeadDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public string Source { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public string? LeadWorkflowDisplayName { get; set; }
    public Guid? LeadWorkflowStageId { get; set; }
    public string? LeadWorkflowStageDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
