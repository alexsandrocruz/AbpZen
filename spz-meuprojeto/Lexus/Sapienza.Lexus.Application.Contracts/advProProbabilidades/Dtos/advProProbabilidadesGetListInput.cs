using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProProbabilidades.Dtos;

[Serializable]
public class advProProbabilidadesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProbabilidade { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
