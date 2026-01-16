using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.fabCondicoesPagamento.Dtos;

[Serializable]
public class fabCondicoesPagamentoGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public string? titulo { get; set; }
    public int? parcelas { get; set; }
    public double? p1 { get; set; }
    public int? d1 { get; set; }
    public double? p2 { get; set; }
    public int? d2 { get; set; }
    public double? p3 { get; set; }
    public int? d3 { get; set; }
    public double? p4 { get; set; }
    public int? d4 { get; set; }
    public double? p5 { get; set; }
    public int? d5 { get; set; }
    public double? p6 { get; set; }
    public int? d6 { get; set; }
    public double? p7 { get; set; }
    public int? d7 { get; set; }
    public double? p8 { get; set; }
    public int? d8 { get; set; }
    public double? p9 { get; set; }
    public int? d9 { get; set; }
    public double? p10 { get; set; }
    public int? d10 { get; set; }
    public double? p11 { get; set; }
    public int? d11 { get; set; }
    public double? p12 { get; set; }
    public int? d12 { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? compras { get; set; }
    public bool? vendas { get; set; }
    public double? valorMinimo { get; set; }
    public bool? atendimento { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? opoOrcamentosId { get; set; }
}
