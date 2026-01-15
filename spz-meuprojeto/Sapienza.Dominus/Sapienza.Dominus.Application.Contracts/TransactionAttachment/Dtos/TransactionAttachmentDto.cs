using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.TransactionAttachment.Dtos;

[Serializable]
public class TransactionAttachmentDto : FullAuditedEntityDto<Guid>
{
    public string FileName { get; set; }
    public string FileUrl { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? TransactionId { get; set; }
    public string? TransactionDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
