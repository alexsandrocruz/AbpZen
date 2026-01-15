using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceInvite.Dtos;

[Serializable]
public class WorkspaceInviteGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
