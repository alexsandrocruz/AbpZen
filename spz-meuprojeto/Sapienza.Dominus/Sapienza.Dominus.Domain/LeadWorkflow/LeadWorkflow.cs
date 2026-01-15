// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadWorkflow;

/// <summary>
/// LeadWorkflow entity
/// </summary>
public class LeadWorkflow : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.LeadWorkflowStage.LeadWorkflowStage> LeadWorkflowStages { get; set; } = new List<Dominus.LeadWorkflowStage.LeadWorkflowStage>();
    public virtual ICollection<Dominus.Lead.Lead> Leads { get; set; } = new List<Dominus.Lead.Lead>();
    public virtual ICollection<Dominus.LeadForm.LeadForm> LeadForms { get; set; } = new List<Dominus.LeadForm.LeadForm>();
    public virtual ICollection<Dominus.LeadLandingPage.LeadLandingPage> LeadLandingPages { get; set; } = new List<Dominus.LeadLandingPage.LeadLandingPage>();
    public virtual ICollection<Dominus.LeadMessageTemplate.LeadMessageTemplate> LeadMessageTemplates { get; set; } = new List<Dominus.LeadMessageTemplate.LeadMessageTemplate>();
    public virtual ICollection<Dominus.LeadAutomation.LeadAutomation> LeadAutomations { get; set; } = new List<Dominus.LeadAutomation.LeadAutomation>();

    protected LeadWorkflow()
    {
        // Required by EF Core
    }

    public LeadWorkflow(Guid id) : base(id)
    {
    }
}
