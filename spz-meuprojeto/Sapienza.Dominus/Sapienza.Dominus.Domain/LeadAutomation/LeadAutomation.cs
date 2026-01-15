// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadAutomation;

/// <summary>
/// LeadAutomation entity
/// </summary>
public class LeadAutomation : FullAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadWorkflow.LeadWorkflow? LeadWorkflow { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.LeadScheduledMessage.LeadScheduledMessage> LeadScheduledMessages { get; set; } = new List<Dominus.LeadScheduledMessage.LeadScheduledMessage>();

    protected LeadAutomation()
    {
        // Required by EF Core
    }

    public LeadAutomation(Guid id) : base(id)
    {
    }
}
