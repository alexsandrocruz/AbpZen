using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabDatasEFeriados.Dtos;

[Serializable]
public class fabDatasEFeriadosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idData { get; set; }
    public string? titulo { get; set; }
    public string? data { get; set; }
    public bool? feriado { get; set; }
    public bool? fixo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
