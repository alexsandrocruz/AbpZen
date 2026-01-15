// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.AiChatMessage;

/// <summary>
/// AiChatMessage entity
/// </summary>
public class AiChatMessage : Entity<Guid>
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? AiChatSessionId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.AiChatSession.AiChatSession? AiChatSession { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected AiChatMessage()
    {
        // Required by EF Core
    }

    public AiChatMessage(Guid id) : base(id)
    {
    }
}
