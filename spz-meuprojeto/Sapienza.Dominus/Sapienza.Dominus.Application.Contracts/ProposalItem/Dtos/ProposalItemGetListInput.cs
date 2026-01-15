using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalItem.Dtos;

[Serializable]
public class ProposalItemGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProposalId { get; set; }
}
