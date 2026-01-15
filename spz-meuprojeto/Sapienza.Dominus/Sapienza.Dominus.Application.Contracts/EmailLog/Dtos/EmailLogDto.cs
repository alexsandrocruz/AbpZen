using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.EmailLog.Dtos;

[Serializable]
public class EmailLogDto : FullAuditedEntityDto<Guid>
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
