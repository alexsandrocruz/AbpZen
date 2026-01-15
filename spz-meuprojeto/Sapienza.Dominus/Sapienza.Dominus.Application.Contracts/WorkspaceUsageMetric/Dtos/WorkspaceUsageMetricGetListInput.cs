using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkspaceUsageMetric.Dtos;

[Serializable]
public class WorkspaceUsageMetricGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
