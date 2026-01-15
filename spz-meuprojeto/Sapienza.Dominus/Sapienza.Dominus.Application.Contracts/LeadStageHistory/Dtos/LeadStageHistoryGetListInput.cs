using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadStageHistory.Dtos;

[Serializable]
public class LeadStageHistoryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LeadId { get; set; }
}
