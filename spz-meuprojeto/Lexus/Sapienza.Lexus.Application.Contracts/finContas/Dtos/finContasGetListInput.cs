using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finContas.Dtos;

[Serializable]
public class finContasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idConta { get; set; }
    public string? titulo { get; set; }
    public string? banco { get; set; }
    public string? agencia { get; set; }
    public string? conta { get; set; }
    public string? favorecido { get; set; }
    public double? limite { get; set; }
    public bool? padraoFluxo { get; set; }
    public bool? considerarIndicador { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public double? saldoInicial { get; set; }
    public bool? padrao { get; set; }
    public string? codigo { get; set; }
    public string? cor { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
