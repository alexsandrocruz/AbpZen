// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Conversation;

/// <summary>
/// Conversation entity
/// </summary>
public class Conversation : FullAuditedAggregateRoot<Guid>
{
    public string Type { get; set; }
    public string Name { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.ChatMessage.ChatMessage> ChatMessages { get; set; } = new List<Dominus.ChatMessage.ChatMessage>();

    protected Conversation()
    {
        // Required by EF Core
    }

    public Conversation(Guid id) : base(id)
    {
    }
}
