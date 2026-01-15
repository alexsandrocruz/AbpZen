using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.AiChatMessage.Dtos;

[Serializable]
public class AiChatMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? AiChatSessionId { get; set; }
}
