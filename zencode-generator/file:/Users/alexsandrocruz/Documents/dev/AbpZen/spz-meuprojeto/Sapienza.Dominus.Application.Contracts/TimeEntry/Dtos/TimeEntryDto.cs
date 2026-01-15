using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.TimeEntry.Dtos;

[Serializable]
public class TimeEntryDto : FullAuditedEntityDto<Guid>
{
    public string Description { get; set; }
    public double Hours { get; set; }
    public DateTime Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }
    public string? ProjectDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
