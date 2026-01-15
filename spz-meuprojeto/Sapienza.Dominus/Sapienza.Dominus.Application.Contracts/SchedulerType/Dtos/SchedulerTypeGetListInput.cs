using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerType.Dtos;

[Serializable]
public class SchedulerTypeGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
