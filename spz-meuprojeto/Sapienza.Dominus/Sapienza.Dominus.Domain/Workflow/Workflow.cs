// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Workflow;

/// <summary>
/// Workflow entity
/// </summary>
public class Workflow : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string TriggerType { get; set; } = string.Empty;
    public string Status { get; set; }
    public string Actions { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.WorkflowExecution.WorkflowExecution> WorkflowExecutions { get; set; } = new List<Dominus.WorkflowExecution.WorkflowExecution>();

    protected Workflow()
    {
        // Required by EF Core
    }

    public Workflow(Guid id) : base(id)
    {
    }
}
