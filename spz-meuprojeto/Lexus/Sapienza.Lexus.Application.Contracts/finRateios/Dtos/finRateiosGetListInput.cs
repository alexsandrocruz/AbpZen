using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finRateios.Dtos;

[Serializable]
public class finRateiosGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idRateio { get; set; }
    public int? idLancamento { get; set; }
    public int? idCentroCusto { get; set; }
    public int? idCentroResultado { get; set; }
    public double? percentualCC { get; set; }
    public double? percentualCR { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUnidade { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
