using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Availability.Dtos;

[Serializable]
public class AvailabilityDto : FullAuditedEntityDto<Guid>
{
    public AvailabilityType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }
    public string? ArtistDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
