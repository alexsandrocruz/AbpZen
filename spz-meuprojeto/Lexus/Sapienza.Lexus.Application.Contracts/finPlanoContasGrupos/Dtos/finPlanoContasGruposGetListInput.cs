using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPlanoContasGrupos.Dtos;

[Serializable]
public class finPlanoContasGruposGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idGrupo { get; set; }
    public string? titulo { get; set; }
    public string? tipo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idGrupoDRE { get; set; }
    public int? ordem { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
