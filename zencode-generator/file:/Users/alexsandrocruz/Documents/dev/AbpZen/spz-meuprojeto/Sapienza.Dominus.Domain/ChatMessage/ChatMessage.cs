// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ChatMessage;

/// <summary>
/// ChatMessage entity
/// </summary>
public class ChatMessage : FullAuditedAggregateRoot<Guid>
{
    public string Content { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ConversationId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Dominus.Conversation.Conversation? Conversation { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Dominus.MessageAttachment.MessageAttachment> MessageAttachments { get; set; } = new List<Sapienza.Dominus.MessageAttachment.MessageAttachment>();

    protected ChatMessage()
    {
        // Required by EF Core
    }

    public ChatMessage(Guid id) : base(id)
    {
    }
}
