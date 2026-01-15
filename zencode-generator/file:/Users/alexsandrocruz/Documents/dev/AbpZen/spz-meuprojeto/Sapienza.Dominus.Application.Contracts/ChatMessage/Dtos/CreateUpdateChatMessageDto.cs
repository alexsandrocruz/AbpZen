using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.ChatMessage.Dtos;

[Serializable]
public class CreateUpdateChatMessageDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ConversationId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
