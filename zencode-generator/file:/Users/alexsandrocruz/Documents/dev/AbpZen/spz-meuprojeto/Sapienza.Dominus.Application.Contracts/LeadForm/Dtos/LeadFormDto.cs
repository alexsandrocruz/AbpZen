using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadForm.Dtos;

[Serializable]
public class LeadFormDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public string? LeadWorkflowDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
