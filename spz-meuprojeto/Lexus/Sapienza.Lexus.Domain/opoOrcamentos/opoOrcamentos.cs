// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.opoOrcamentos;

/// <summary>
/// opoOrcamentos entity
/// </summary>
public class opoOrcamentos : FullAuditedAggregateRoot<Guid>
{
    public int? idOrcamento { get; set; }
    public int idOportunidade { get; set; }
    public string? titulo { get; set; }
    public string? dataCriacao { get; set; }
    public double? valor { get; set; }
    public string? arquivo { get; set; }
    public bool? aceito { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? dataValidade { get; set; }
    public double? valorMensal { get; set; }
    public bool? comArquivo { get; set; }
    public bool? comProduto { get; set; }
    public bool? comProdutoTerceiro { get; set; }
    public bool? comServico { get; set; }
    public double? valorDesconto { get; set; }
    public double? valorAcrescimo { get; set; }
    public double? valorFrete { get; set; }
    public string? informacoes { get; set; }
    public double? descontoPercentual { get; set; }
    public double? valorItens { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public string? dataPrevistaEntrega { get; set; }
    public string? moeda { get; set; }
    public double? valorConversao { get; set; }
    public string? imprimeMoedaAdd { get; set; }
    public double? valorDescontoMensal { get; set; }
    public double? valorAcrescimoMensal { get; set; }
    public double? valorFreteMensal { get; set; }
    public double? descontoPercentualMensal { get; set; }
    public double? valorItensMensal { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento> opoOrcamentoses { get; set; } = new List<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento>();
    public virtual ICollection<Sapienza.Lexus.opoOportunidades.opoOportunidades> opoOrcamentosesCollection { get; set; } = new List<Sapienza.Lexus.opoOportunidades.opoOportunidades>();

    protected opoOrcamentos()
    {
        // Required by EF Core
    }

    public opoOrcamentos(Guid id) : base(id)
    {
    }
}
