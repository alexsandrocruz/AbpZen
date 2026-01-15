using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.MessageAttachment.Dtos;

[Serializable]
public class MessageAttachmentGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ChatMessageId { get; set; }
}
