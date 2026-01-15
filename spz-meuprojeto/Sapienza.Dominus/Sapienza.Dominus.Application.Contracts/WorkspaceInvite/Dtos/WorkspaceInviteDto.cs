using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceInvite.Dtos;

[Serializable]
public class WorkspaceInviteDto : FullAuditedEntityDto<Guid>
{
    public string Email { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
