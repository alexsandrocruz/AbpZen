using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadLandingPage.Dtos;

[Serializable]
public class LeadLandingPageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
}
