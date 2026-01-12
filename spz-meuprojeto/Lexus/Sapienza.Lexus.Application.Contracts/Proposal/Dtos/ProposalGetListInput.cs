#nullable enable
using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.Proposal.Dtos;

[Serializable]
public class ProposalGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? Validate { get; set; }
    public string? Obs { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
