// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.AiChatSession;

/// <summary>
/// AiChatSession entity
/// </summary>
public class AiChatSession : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string ContextType { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.AiChatMessage.AiChatMessage> AiChatMessages { get; set; } = new List<Dominus.AiChatMessage.AiChatMessage>();

    protected AiChatSession()
    {
        // Required by EF Core
    }

    public AiChatSession(Guid id) : base(id)
    {
    }
}
