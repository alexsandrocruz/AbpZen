using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.PropostalItem.Dtos;

[Serializable]
public class PropostalItemGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Desc { get; set; }
    public decimal? Quant { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Total { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProposalId { get; set; }
}
