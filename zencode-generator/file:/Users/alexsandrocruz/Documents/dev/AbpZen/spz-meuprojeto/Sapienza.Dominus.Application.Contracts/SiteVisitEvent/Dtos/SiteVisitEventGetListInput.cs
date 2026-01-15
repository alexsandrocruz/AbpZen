using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SiteVisitEvent.Dtos;

[Serializable]
public class SiteVisitEventGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SiteId { get; set; }
}
