using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.WorkflowExecution.Dtos;

[Serializable]
public class CreateUpdateWorkflowExecutionDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? WorkflowId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
