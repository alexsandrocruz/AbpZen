using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProposalTemplateBlock.Dtos;

[Serializable]
public class ProposalTemplateBlockDto : FullAuditedEntityDto<Guid>
{
    public string BlockType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
