using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ProjectCommunication.Dtos;

[Serializable]
public class ProjectCommunicationDto : FullAuditedEntityDto<Guid>
{
    public string Channel { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }
    public string? ProjectDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
