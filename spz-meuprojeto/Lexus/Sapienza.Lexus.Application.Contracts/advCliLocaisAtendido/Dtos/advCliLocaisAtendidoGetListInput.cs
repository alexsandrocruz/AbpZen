using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliLocaisAtendido.Dtos;

[Serializable]
public class advCliLocaisAtendidoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idLocalAtendido { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
