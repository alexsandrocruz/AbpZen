using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectFollower.Dtos;

[Serializable]
public class ProjectFollowerGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProjectId { get; set; }
}
