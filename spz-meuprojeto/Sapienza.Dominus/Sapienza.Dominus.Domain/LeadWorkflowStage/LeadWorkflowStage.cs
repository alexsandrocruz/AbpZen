// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadWorkflowStage;

/// <summary>
/// LeadWorkflowStage entity
/// </summary>
public class LeadWorkflowStage : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.Lead.Lead> Leads { get; set; } = new List<Dominus.Lead.Lead>();

    protected LeadWorkflowStage()
    {
        // Required by EF Core
    }

    public LeadWorkflowStage(Guid id) : base(id)
    {
    }
}
