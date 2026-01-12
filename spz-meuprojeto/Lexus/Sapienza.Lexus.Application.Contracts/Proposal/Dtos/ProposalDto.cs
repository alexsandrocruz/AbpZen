#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.Proposal.Dtos;

[Serializable]
public class ProposalDto : FullAuditedEntityDto<Guid>
{
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ValidityDate { get; set; }
    public string? Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
