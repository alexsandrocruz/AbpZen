using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Proposal.Dtos;

[Serializable]
public class ProposalGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
