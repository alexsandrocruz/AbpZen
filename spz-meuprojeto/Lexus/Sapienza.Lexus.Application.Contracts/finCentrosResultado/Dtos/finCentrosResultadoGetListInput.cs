using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finCentrosResultado.Dtos;

[Serializable]
public class finCentrosResultadoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idCentroResultado { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padrao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
