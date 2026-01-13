using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finProcuracoesRPV.Dtos;

[Serializable]
public class finProcuracoesRPVGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idProcuracao { get; set; }
    public int? idCliente { get; set; }
    public int? idProcesso { get; set; }
    public bool? impressa { get; set; }
    public DateTime? tsImpressa { get; set; }
    public bool? assinada { get; set; }
    public DateTime? tsAssinatura { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
