using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ChatMessage.Dtos;

[Serializable]
public class ChatMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ConversationId { get; set; }
}
