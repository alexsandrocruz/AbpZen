using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SiteVisitEvent.Dtos;

[Serializable]
public class SiteVisitEventDto : FullAuditedEntityDto<Guid>
{
    public string VisitorId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SiteId { get; set; }
    public string? SiteDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
