using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRecibos.Dtos;

[Serializable]
public class finRecibosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idRecibo { get; set; }
    public int? idLancamento { get; set; }
    public int? numero { get; set; }
    public string? referente { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
