using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceAccessEvent.Dtos;

[Serializable]
public class WorkspaceAccessEventDto : FullAuditedEntityDto<Guid>
{
    public string EventType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
