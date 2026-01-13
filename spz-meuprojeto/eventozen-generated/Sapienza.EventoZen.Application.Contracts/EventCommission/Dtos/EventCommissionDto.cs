using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.EventCommission.Dtos;

[Serializable]
public class EventCommissionDto : FullAuditedEntityDto<Guid>
{
    public string Description { get; set; }
    public decimal? Value { get; set; }
    public decimal? Percentage { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? EventId { get; set; }
    public string? EventDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
