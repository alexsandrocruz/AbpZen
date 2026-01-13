using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProfissionaisNaturezas.Dtos;

[Serializable]
public class advProfissionaisNaturezasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProfissionalNatureza { get; set; }
    public int? idProfissional { get; set; }
    public int? idNatureza { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
