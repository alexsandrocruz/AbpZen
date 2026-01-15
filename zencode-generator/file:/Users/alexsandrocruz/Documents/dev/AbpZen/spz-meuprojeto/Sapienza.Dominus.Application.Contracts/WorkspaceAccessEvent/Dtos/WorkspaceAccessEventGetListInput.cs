using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceAccessEvent.Dtos;

[Serializable]
public class WorkspaceAccessEventGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
