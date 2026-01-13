using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProTipos.Dtos;

[Serializable]
public class advProTiposGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
