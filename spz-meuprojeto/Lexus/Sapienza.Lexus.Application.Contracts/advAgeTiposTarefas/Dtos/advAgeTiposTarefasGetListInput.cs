using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advAgeTiposTarefas.Dtos;

[Serializable]
public class advAgeTiposTarefasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idTipoTarefa { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? agendada { get; set; }
    public bool? pauta { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
