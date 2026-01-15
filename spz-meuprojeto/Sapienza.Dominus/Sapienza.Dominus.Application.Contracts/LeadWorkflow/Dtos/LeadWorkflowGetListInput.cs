using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadWorkflow.Dtos;

[Serializable]
public class LeadWorkflowGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
