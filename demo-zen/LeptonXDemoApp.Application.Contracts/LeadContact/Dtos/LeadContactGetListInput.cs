using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.LeadContact.Dtos;

[Serializable]
public class LeadContactGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadId { get; set; }
}
