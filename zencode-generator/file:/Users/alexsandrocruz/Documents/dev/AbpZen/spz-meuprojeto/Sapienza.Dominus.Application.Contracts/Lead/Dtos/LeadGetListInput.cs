using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Lead.Dtos;

[Serializable]
public class LeadGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
    public Guid? LeadWorkflowStageId { get; set; }
}
