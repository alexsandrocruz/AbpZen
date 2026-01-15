using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SitePageVersion.Dtos;

[Serializable]
public class SitePageVersionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SitePageId { get; set; }
}
