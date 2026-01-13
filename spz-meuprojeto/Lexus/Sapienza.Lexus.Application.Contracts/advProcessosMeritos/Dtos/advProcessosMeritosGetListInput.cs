using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProcessosMeritos.Dtos;

[Serializable]
public class advProcessosMeritosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProcessoMerito { get; set; }
    public int? idProcesso { get; set; }
    public int? idMerito { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
