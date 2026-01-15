using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadAutomation.Dtos;

[Serializable]
public class LeadAutomationDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public string? LeadWorkflowDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
