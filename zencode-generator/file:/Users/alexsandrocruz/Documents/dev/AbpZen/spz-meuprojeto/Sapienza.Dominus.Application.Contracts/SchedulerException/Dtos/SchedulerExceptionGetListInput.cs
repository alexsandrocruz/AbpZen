using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerException.Dtos;

[Serializable]
public class SchedulerExceptionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SchedulerTypeId { get; set; }
}
