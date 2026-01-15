using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WhatsappLog.Dtos;

[Serializable]
public class WhatsappLogDto : FullAuditedEntityDto<Guid>
{
    public string ToPhone { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
