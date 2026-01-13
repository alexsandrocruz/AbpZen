using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.opoOrcamentos.ViewModels;

public class EditopoOrcamentosViewModel
{
    [Display(Name = "opoOrcamentos:idOrcamento")]
    public int? idOrcamento { get; set; }
    [Required]
    [Display(Name = "opoOrcamentos:idOportunidade")]
    public int idOportunidade { get; set; }
    [Display(Name = "opoOrcamentos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "opoOrcamentos:dataCriacao")]
    public string? dataCriacao { get; set; }
    [Display(Name = "opoOrcamentos:valor")]
    public double? valor { get; set; }
    [Display(Name = "opoOrcamentos:arquivo")]
    public string? arquivo { get; set; }
    [Display(Name = "opoOrcamentos:aceito")]
    public bool? aceito { get; set; }
    [Display(Name = "opoOrcamentos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "opoOrcamentos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "opoOrcamentos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "opoOrcamentos:dataValidade")]
    public string? dataValidade { get; set; }
    [Display(Name = "opoOrcamentos:valorMensal")]
    public double? valorMensal { get; set; }
    [Display(Name = "opoOrcamentos:comArquivo")]
    public bool? comArquivo { get; set; }
    [Display(Name = "opoOrcamentos:comProduto")]
    public bool? comProduto { get; set; }
    [Display(Name = "opoOrcamentos:comProdutoTerceiro")]
    public bool? comProdutoTerceiro { get; set; }
    [Display(Name = "opoOrcamentos:comServico")]
    public bool? comServico { get; set; }
    [Display(Name = "opoOrcamentos:valorDesconto")]
    public double? valorDesconto { get; set; }
    [Display(Name = "opoOrcamentos:valorAcrescimo")]
    public double? valorAcrescimo { get; set; }
    [Display(Name = "opoOrcamentos:valorFrete")]
    public double? valorFrete { get; set; }
    [Display(Name = "opoOrcamentos:informacoes")]
    public string? informacoes { get; set; }
    [Display(Name = "opoOrcamentos:descontoPercentual")]
    public double? descontoPercentual { get; set; }
    [Display(Name = "opoOrcamentos:valorItens")]
    public double? valorItens { get; set; }
    [Display(Name = "opoOrcamentos:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [Display(Name = "opoOrcamentos:dataPrevistaEntrega")]
    public string? dataPrevistaEntrega { get; set; }
    [Display(Name = "opoOrcamentos:moeda")]
    public string? moeda { get; set; }
    [Display(Name = "opoOrcamentos:valorConversao")]
    public double? valorConversao { get; set; }
    [Display(Name = "opoOrcamentos:imprimeMoedaAdd")]
    public string? imprimeMoedaAdd { get; set; }
    [Display(Name = "opoOrcamentos:valorDescontoMensal")]
    public double? valorDescontoMensal { get; set; }
    [Display(Name = "opoOrcamentos:valorAcrescimoMensal")]
    public double? valorAcrescimoMensal { get; set; }
    [Display(Name = "opoOrcamentos:valorFreteMensal")]
    public double? valorFreteMensal { get; set; }
    [Display(Name = "opoOrcamentos:descontoPercentualMensal")]
    public double? descontoPercentualMensal { get; set; }
    [Display(Name = "opoOrcamentos:valorItensMensal")]
    public double? valorItensMensal { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
