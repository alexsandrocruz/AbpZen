using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Lead.Dtos;

[Serializable]
public class LeadGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
