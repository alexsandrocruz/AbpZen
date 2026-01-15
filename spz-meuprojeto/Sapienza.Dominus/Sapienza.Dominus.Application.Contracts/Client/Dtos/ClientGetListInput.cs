using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Client.Dtos;

[Serializable]
public class ClientGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
