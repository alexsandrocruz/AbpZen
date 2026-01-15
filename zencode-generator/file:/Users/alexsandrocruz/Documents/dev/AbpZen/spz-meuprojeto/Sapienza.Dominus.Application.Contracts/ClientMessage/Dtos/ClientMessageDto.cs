using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ClientMessage.Dtos;

[Serializable]
public class ClientMessageDto : FullAuditedEntityDto<Guid>
{
    public string Channel { get; set; }
    public string Direction { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
