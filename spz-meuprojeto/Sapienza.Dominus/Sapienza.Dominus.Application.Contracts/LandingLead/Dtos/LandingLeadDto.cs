using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LandingLead.Dtos;

[Serializable]
public class LandingLeadDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
