using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectResponsible.Dtos;

[Serializable]
public class ProjectResponsibleDto : FullAuditedEntityDto<Guid>
{

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }
    public string? ProjectDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
