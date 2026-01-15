using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.AiChatMessage.Dtos;

[Serializable]
public class AiChatMessageDto : FullAuditedEntityDto<Guid>
{
    public string Role { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? AiChatSessionId { get; set; }
    public string? AiChatSessionDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
