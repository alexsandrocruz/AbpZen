using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Task.Dtos;

[Serializable]
public class TaskGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProjectId { get; set; }
}
