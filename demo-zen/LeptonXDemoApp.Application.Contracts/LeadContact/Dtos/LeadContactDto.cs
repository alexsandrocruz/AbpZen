using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.LeadContact.Dtos;

[Serializable]
public class LeadContactDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadId { get; set; }
    public string? LeadDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
