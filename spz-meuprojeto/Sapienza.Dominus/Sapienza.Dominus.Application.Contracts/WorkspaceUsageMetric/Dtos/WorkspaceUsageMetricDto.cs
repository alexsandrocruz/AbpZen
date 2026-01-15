using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceUsageMetric.Dtos;

[Serializable]
public class WorkspaceUsageMetricDto : FullAuditedEntityDto<Guid>
{
    public DateTime Date { get; set; }
    public int TotalSessions { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
