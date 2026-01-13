using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProEscritorios.Dtos;

[Serializable]
public class advProEscritoriosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idEscritorio { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCentroCusto { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
