using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPlanoContasDet.Dtos;

[Serializable]
public class finPlanoContasDetGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPlanoContasDet { get; set; }
    public int? idPlanoConta { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
