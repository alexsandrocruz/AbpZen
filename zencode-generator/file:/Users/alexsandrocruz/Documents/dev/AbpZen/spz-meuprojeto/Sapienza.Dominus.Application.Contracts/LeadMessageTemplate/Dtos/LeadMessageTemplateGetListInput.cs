using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadMessageTemplate.Dtos;

[Serializable]
public class LeadMessageTemplateGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadWorkflowId { get; set; }
}
