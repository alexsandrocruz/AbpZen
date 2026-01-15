using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SitePage.Dtos;

[Serializable]
public class SitePageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SiteId { get; set; }
}
