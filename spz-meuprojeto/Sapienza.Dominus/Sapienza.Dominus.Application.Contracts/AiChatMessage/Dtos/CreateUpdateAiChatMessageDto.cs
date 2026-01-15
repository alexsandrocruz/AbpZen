using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.AiChatMessage.Dtos;

[Serializable]
public class CreateUpdateAiChatMessageDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Role { get; set; }
    [Required]
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? AiChatSessionId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
