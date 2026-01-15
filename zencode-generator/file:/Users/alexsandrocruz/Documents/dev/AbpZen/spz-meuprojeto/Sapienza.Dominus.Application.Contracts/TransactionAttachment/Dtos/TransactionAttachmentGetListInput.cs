using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.TransactionAttachment.Dtos;

[Serializable]
public class TransactionAttachmentGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? TransactionId { get; set; }
}
