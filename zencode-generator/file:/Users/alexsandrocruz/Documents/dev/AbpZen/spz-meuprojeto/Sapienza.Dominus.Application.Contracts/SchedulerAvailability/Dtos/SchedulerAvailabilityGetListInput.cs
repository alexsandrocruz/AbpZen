using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerAvailability.Dtos;

[Serializable]
public class SchedulerAvailabilityGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SchedulerTypeId { get; set; }
}
