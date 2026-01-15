// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadScheduledMessage;

/// <summary>
/// LeadScheduledMessage entity
/// </summary>
public class LeadScheduledMessage : Entity<Guid>
{
    public DateTime ScheduledFor { get; set; }
    public string Status { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadAutomationId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.LeadAutomation.LeadAutomation? LeadAutomation { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadScheduledMessage()
    {
        // Required by EF Core
    }

    public LeadScheduledMessage(Guid id) : base(id)
    {
    }
}
