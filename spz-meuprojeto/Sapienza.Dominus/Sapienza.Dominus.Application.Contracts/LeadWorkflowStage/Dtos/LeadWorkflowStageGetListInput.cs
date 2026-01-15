using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadWorkflowStage.Dtos;

[Serializable]
public class LeadWorkflowStageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
}
