using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.TimeEntry.Dtos;

[Serializable]
public class TimeEntryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProjectId { get; set; }
}
