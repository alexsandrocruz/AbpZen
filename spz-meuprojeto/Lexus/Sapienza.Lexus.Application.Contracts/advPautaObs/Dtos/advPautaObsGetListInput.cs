using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advPautaObs.Dtos;

[Serializable]
public class advPautaObsGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPautaObs { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? id { get; set; }
    public string? idTipo { get; set; }
    public string? observacao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
