using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finAreas.Dtos;

[Serializable]
public class finAreasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idArea { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCentroResultado { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? finLancamentosId { get; set; }
}
