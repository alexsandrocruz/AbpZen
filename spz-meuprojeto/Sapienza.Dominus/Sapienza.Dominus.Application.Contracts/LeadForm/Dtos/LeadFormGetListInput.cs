using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadForm.Dtos;

[Serializable]
public class LeadFormGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
}
