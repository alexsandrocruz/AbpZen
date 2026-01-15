using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalBlockInstance.Dtos;

[Serializable]
public class ProposalBlockInstanceGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProposalId { get; set; }
}
