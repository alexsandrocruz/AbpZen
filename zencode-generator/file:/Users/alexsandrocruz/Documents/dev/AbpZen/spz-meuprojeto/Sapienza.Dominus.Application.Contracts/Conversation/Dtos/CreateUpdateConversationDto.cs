using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Conversation.Dtos;

[Serializable]
public class CreateUpdateConversationDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
