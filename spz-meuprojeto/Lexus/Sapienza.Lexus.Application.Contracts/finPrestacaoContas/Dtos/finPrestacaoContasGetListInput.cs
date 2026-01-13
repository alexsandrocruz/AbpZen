using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPrestacaoContas.Dtos;

[Serializable]
public class finPrestacaoContasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPrestacao { get; set; }
    public int? idLancamento { get; set; }
    public double? levantado { get; set; }
    public double? irpj { get; set; }
    public double? carta { get; set; }
    public double? honorarios { get; set; }
    public double? tarifa { get; set; }
    public double? liquidoRecebido { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
