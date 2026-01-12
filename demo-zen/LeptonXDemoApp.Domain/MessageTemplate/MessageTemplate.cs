using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.MessageTemplate;

/// <summary>
/// MessageTemplate entity
/// </summary>
public class MessageTemplate : FullAuditedAggregateRoot<Guid>
{
    public string? Title { get; set; }
    public string? Body { get; set; }
    public MessageTypeEnum? MessageType { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.LeadMessage.LeadMessage> LeadMessages { get; set; } = new List<LeptonXDemoApp.LeadMessage.LeadMessage>();

    protected MessageTemplate()
    {
        // Required by EF Core
    }

    public MessageTemplate(Guid id) : base(id)
    {
    }
}
