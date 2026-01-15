using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalBlockInstance.Dtos;

[Serializable]
public class ProposalBlockInstanceDto : FullAuditedEntityDto<Guid>
{
    public string BlockType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }
    public string? ProposalDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
