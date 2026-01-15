using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.MessageAttachment.Dtos;

[Serializable]
public class MessageAttachmentDto : FullAuditedEntityDto<Guid>
{
    public string FileName { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ChatMessageId { get; set; }
    public string? ChatMessageDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
