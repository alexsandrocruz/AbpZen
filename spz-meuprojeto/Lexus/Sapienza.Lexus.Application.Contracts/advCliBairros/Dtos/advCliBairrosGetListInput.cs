using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliBairros.Dtos;

[Serializable]
public class advCliBairrosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idBairro { get; set; }
    public string? titulo { get; set; }
    public string? cidade { get; set; }
    public string? estado { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
