using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectCommunication.Dtos;

[Serializable]
public class ProjectCommunicationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ProjectId { get; set; }
}
