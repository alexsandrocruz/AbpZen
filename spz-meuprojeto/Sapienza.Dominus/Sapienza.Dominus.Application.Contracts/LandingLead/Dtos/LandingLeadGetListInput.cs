using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LandingLead.Dtos;

[Serializable]
public class LandingLeadGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
