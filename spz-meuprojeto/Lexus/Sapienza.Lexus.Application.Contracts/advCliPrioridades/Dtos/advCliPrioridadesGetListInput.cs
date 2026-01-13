using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advCliPrioridades.Dtos;

[Serializable]
public class advCliPrioridadesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPrioridade { get; set; }
    public string? titulo { get; set; }
    public string? cor { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
