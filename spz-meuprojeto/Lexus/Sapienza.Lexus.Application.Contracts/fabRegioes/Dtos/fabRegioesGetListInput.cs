using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabRegioes.Dtos;

[Serializable]
public class fabRegioesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idRegiao { get; set; }
    public string? titulo { get; set; }
    public string? estados { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
