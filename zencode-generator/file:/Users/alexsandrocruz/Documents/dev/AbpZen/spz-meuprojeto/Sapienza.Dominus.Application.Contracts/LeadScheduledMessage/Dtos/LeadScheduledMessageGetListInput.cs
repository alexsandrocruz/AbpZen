using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadScheduledMessage.Dtos;

[Serializable]
public class LeadScheduledMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadAutomationId { get; set; }
}
