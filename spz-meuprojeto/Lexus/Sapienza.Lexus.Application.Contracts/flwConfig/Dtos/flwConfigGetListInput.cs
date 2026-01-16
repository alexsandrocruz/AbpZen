using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwConfig.Dtos;

[Serializable]
public class flwConfigGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idConfig { get; set; }
    public string? tipoMarcacoes { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
