using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.SchedulerException.Dtos;

[Serializable]
public class SchedulerExceptionDto : FullAuditedEntityDto<Guid>
{
    public DateTime Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SchedulerTypeId { get; set; }
    public string? SchedulerTypeDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
