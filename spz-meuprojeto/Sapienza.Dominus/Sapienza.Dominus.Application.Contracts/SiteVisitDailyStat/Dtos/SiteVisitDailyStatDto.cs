using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SiteVisitDailyStat.Dtos;

[Serializable]
public class SiteVisitDailyStatDto : FullAuditedEntityDto<Guid>
{
    public DateTime Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SiteId { get; set; }
    public string? SiteDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
