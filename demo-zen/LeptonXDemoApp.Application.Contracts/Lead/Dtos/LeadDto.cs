using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Lead.Dtos;

[Serializable]
public class LeadDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
