using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerAvailability.Dtos;

[Serializable]
public class SchedulerAvailabilityDto : FullAuditedEntityDto<Guid>
{
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SchedulerTypeId { get; set; }
    public string? SchedulerTypeDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
