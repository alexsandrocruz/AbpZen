using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finUnidades.Dtos;

[Serializable]
public class finUnidadesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idUnidade { get; set; }
    public string? titulo { get; set; }
    public double? percentual { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
