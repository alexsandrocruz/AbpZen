using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Project.Dtos;

[Serializable]
public class ProjectGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
