using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.WhatsappLog.Dtos;

[Serializable]
public class WhatsappLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
