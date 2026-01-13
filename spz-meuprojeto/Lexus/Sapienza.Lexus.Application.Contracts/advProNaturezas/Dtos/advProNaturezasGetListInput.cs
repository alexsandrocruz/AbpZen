using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advProNaturezas.Dtos;

[Serializable]
public class advProNaturezasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idNatureza { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? mostraHistoricoNumeros { get; set; }
    public bool? recebeAcordo { get; set; }
    public bool? recebeRPV { get; set; }
    public bool? recebePrecatorio { get; set; }
    public bool? recebeAlvara { get; set; }
    public int? idArea { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
