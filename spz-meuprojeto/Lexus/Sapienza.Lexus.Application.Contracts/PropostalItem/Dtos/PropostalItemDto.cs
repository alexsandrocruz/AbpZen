#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.PropostalItem.Dtos;

[Serializable]
public class PropostalItemDto : FullAuditedEntityDto<Guid>
{
    public string? Desc { get; set; }
    public decimal? Quant { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }
    public string? ProposalDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
