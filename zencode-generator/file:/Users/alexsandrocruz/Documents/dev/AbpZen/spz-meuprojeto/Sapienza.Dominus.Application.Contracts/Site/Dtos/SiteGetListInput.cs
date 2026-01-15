using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Site.Dtos;

[Serializable]
public class SiteGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
