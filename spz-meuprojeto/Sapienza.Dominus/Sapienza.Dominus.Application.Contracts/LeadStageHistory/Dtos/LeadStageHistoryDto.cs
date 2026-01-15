using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadStageHistory.Dtos;

[Serializable]
public class LeadStageHistoryDto : FullAuditedEntityDto<Guid>
{
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadId { get; set; }
    public string? LeadDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
