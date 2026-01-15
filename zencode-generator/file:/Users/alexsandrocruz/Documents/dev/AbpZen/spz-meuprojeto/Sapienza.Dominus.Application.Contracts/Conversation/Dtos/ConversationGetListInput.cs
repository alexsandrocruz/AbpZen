using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Conversation.Dtos;

[Serializable]
public class ConversationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
