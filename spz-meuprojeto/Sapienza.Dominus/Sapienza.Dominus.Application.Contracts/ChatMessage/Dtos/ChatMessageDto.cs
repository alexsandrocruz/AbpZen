using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ChatMessage.Dtos;

[Serializable]
public class ChatMessageDto : FullAuditedEntityDto<Guid>
{
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ConversationId { get; set; }
    public string? ConversationDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
