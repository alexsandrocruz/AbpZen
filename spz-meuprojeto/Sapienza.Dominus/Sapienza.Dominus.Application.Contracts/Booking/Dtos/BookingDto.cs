using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Booking.Dtos;

[Serializable]
public class BookingDto : FullAuditedEntityDto<Guid>
{
    public string ClientName { get; set; }
    public string ClientEmail { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SchedulerTypeId { get; set; }
    public string? SchedulerTypeDisplayName { get; set; }
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
