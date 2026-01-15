using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.AiChatSession.Dtos;

[Serializable]
public class AiChatSessionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
