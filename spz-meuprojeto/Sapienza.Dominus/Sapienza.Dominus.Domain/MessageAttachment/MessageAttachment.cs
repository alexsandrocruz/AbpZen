// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.MessageAttachment;

/// <summary>
/// MessageAttachment entity
/// </summary>
public class MessageAttachment : Entity<Guid>
{
    public string FileName { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ChatMessageId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.ChatMessage.ChatMessage? ChatMessage { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected MessageAttachment()
    {
        // Required by EF Core
    }

    public MessageAttachment(Guid id) : base(id)
    {
    }
}
