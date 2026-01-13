using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.opoOrcamentos.Dtos;

[Serializable]
public class CreateUpdateopoOrcamentosDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idOrcamento { get; set; }
    [Required]
    public int idOportunidade { get; set; }
    public string titulo { get; set; }
    public string dataCriacao { get; set; }
    public double? valor { get; set; }
    public string arquivo { get; set; }
    public bool? aceito { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string dataValidade { get; set; }
    public double? valorMensal { get; set; }
    public bool? comArquivo { get; set; }
    public bool? comProduto { get; set; }
    public bool? comProdutoTerceiro { get; set; }
    public bool? comServico { get; set; }
    public double? valorDesconto { get; set; }
    public double? valorAcrescimo { get; set; }
    public double? valorFrete { get; set; }
    public string informacoes { get; set; }
    public double? descontoPercentual { get; set; }
    public double? valorItens { get; set; }
    public int? idCondicaoPagamento { get; set; }
    public string dataPrevistaEntrega { get; set; }
    public string moeda { get; set; }
    public double? valorConversao { get; set; }
    public string imprimeMoedaAdd { get; set; }
    public double? valorDescontoMensal { get; set; }
    public double? valorAcrescimoMensal { get; set; }
    public double? valorFreteMensal { get; set; }
    public double? descontoPercentualMensal { get; set; }
    public double? valorItensMensal { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
