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
    public virtual ICollection<Sapienza.Dominus.LeadWorkflowStage.LeadWorkflowStage> LeadWorkflowStages { get; set; } = new List<Sapienza.Dominus.LeadWorkflowStage.LeadWorkflowStage>();
    public virtual ICollection<Sapienza.Dominus.Lead.Lead> Leads { get; set; } = new List<Sapienza.Dominus.Lead.Lead>();
    public virtual ICollection<Sapienza.Dominus.LeadForm.LeadForm> LeadForms { get; set; } = new List<Sapienza.Dominus.LeadForm.LeadForm>();
    public virtual ICollection<Sapienza.Dominus.LeadLandingPage.LeadLandingPage> LeadLandingPages { get; set; } = new List<Sapienza.Dominus.LeadLandingPage.LeadLandingPage>();
    public virtual ICollection<Sapienza.Dominus.LeadMessageTemplate.LeadMessageTemplate> LeadMessageTemplates { get; set; } = new List<Sapienza.Dominus.LeadMessageTemplate.LeadMessageTemplate>();
    public virtual ICollection<Sapienza.Dominus.LeadAutomation.LeadAutomation> LeadAutomations { get; set; } = new List<Sapienza.Dominus.LeadAutomation.LeadAutomation>();

    protected LeadWorkflow()
    {
        // Required by EF Core
    }

    public LeadWorkflow(Guid id) : base(id)
    {
    }
}
