using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WorkflowExecution.Dtos;

[Serializable]
public class WorkflowExecutionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? WorkflowId { get; set; }
}
