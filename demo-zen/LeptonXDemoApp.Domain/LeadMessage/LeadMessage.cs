using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.LeadMessage;

/// <summary>
/// LeadMessage entity
/// </summary>
public class LeadMessage : FullAuditedAggregateRoot<Guid>
{
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public string? Body { get; set; }
    public bool? Success { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? MessageTemplateId { get; set; }
    public Guid? LeadId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.MessageTemplate.MessageTemplate? MessageTemplate { get; set; }
    public virtual LeptonXDemoApp.Lead.Lead? Lead { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadMessage()
    {
        // Required by EF Core
    }

    public LeadMessage(Guid id) : base(id)
    {
    }
}
