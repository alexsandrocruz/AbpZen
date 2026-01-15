using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.TaskComment.Dtos;

[Serializable]
public class TaskCommentDto : FullAuditedEntityDto<Guid>
{
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? TaskId { get; set; }
    public string? TaskDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
