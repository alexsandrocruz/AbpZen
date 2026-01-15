using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.AiChatSession.Dtos;

[Serializable]
public class AiChatSessionDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string ContextType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
