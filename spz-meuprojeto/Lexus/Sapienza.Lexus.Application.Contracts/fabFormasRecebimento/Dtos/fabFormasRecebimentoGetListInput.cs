using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabFormasRecebimento.Dtos;

[Serializable]
public class fabFormasRecebimentoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idFormaRecebimento { get; set; }
    public string? titulo { get; set; }
    public int? ordem { get; set; }
    public bool? padrao { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public bool? online { get; set; }
    public string? tipo { get; set; }
    public string? emailPagSeguro { get; set; }
    public string? texto { get; set; }
    public bool? contasReceber { get; set; }
    public bool? vendas { get; set; }
    public int? diasParaPrevisao { get; set; }
    public double? valorDesconto { get; set; }
    public string? descontoTipo { get; set; }
    public bool? recebimentoFuturo { get; set; }
    public int? recebimentoFuturoDias { get; set; }
    public double? recebimentoFuturoTaxa { get; set; }
    public int? idConta { get; set; }
    public int? idPlanoConta { get; set; }
    public int? idCentroCusto { get; set; }
    public int? idContaPagar { get; set; }
    public int? idPlanoContaPagar { get; set; }
    public int? idCentroCustoPagar { get; set; }
    public int? idFormaPagar { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
