using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Comment.Dtos;

[Serializable]
public class CommentDto : FullAuditedEntityDto<Guid>
{
    public string EntityType { get; set; }
    public string Content { get; set; }
    public string EntityId { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
