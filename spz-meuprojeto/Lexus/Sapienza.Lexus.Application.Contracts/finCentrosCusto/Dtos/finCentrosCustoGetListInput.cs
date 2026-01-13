using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finCentrosCusto.Dtos;

[Serializable]
public class finCentrosCustoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idCentroCusto { get; set; }
    public int? idUnidade { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padrao { get; set; }
    public double? porcentagemRateio { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
