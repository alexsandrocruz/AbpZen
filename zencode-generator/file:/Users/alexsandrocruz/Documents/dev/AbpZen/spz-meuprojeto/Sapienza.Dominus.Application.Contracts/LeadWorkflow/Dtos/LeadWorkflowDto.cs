using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadWorkflow.Dtos;

[Serializable]
public class LeadWorkflowDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
