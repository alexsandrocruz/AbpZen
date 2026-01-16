using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabFormasPagamento.Dtos;

[Serializable]
public class fabFormasPagamentoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idFormaPagamento { get; set; }
    public string? titulo { get; set; }
    public int? ordem { get; set; }
    public bool? padrao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public bool? contasPagar { get; set; }
    public bool? compras { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
