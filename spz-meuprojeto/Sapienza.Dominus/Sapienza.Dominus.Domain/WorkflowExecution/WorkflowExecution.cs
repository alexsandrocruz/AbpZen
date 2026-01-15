// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.WorkflowExecution;

/// <summary>
/// WorkflowExecution entity
/// </summary>
public class WorkflowExecution : Entity<Guid>
{
    public string Status { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? WorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Workflow.Workflow? Workflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected WorkflowExecution()
    {
        // Required by EF Core
    }

    public WorkflowExecution(Guid id) : base(id)
    {
    }
}
