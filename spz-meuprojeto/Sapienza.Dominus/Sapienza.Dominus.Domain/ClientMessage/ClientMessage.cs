// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ClientMessage;

/// <summary>
/// ClientMessage entity
/// </summary>
public class ClientMessage : FullAuditedEntity<Guid>
{
    public string Channel { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string Subject { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ClientMessage()
    {
        // Required by EF Core
    }

    public ClientMessage(Guid id) : base(id)
    {
    }
}
