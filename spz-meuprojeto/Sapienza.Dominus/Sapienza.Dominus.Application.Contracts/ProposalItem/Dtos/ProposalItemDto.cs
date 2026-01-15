using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalItem.Dtos;

[Serializable]
public class ProposalItemDto : FullAuditedEntityDto<Guid>
{
    public string Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }
    public string? ProposalDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
