using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.ClientMessage.Dtos;

[Serializable]
public class ClientMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
}
