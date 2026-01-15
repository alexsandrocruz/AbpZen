// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Lead;

/// <summary>
/// Lead entity
/// </summary>
public class Lead : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public string Source { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }
    public Guid? LeadWorkflowStageId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }
    public virtual Dominus.LeadWorkflowStage.LeadWorkflowStage? LeadWorkflowStage { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.LeadStageHistory.LeadStageHistory> LeadStageHistories { get; set; } = new List<Dominus.LeadStageHistory.LeadStageHistory>();

    protected Lead()
    {
        // Required by EF Core
    }

    public Lead(Guid id) : base(id)
    {
    }
}
