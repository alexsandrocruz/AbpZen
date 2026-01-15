using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerType.Dtos;

[Serializable]
public class SchedulerTypeDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public int DurationMinutes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
