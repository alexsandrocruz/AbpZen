using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectResponsible.Dtos;

[Serializable]
public class ProjectResponsibleGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProjectId { get; set; }
}
