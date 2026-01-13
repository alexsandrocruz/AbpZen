using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.flwAcoes.Dtos;

[Serializable]
public class flwAcoesGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idAcao { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? diasReagendamento { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
