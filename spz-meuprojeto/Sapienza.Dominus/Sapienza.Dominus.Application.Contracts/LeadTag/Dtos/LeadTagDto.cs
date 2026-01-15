using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadTag.Dtos;

[Serializable]
public class LeadTagDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Color { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
