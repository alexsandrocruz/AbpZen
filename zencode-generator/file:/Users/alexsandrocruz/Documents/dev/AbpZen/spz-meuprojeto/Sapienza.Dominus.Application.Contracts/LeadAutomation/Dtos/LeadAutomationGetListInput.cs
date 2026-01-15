using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadAutomation.Dtos;

[Serializable]
public class LeadAutomationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
}
