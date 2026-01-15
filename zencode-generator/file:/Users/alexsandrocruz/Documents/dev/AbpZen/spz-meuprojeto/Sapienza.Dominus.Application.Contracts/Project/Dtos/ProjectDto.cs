using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Project.Dtos;

[Serializable]
public class ProjectDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public double Budget { get; set; }
    public DateTime DueDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
