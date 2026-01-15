using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Task.Dtos;

[Serializable]
public class TaskDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }
    public string? ProjectDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
