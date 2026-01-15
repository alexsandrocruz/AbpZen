using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectFollower.Dtos;

[Serializable]
public class ProjectFollowerDto : FullAuditedEntityDto<Guid>
{

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }
    public string? ProjectDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
