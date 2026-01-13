using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRateiosPadrao.Dtos;

[Serializable]
public class finRateiosPadraoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPadrao { get; set; }
    public int? idUnidade { get; set; }
    public int? idCentroResultado { get; set; }
    public double? porcentagem { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
