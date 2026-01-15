using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SiteVisitDailyStat.Dtos;

[Serializable]
public class SiteVisitDailyStatGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SiteId { get; set; }
}
