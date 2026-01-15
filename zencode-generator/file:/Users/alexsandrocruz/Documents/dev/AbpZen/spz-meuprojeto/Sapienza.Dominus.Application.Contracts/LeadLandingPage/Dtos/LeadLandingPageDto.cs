using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadLandingPage.Dtos;

[Serializable]
public class LeadLandingPageDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Slug { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public string? LeadWorkflowDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
