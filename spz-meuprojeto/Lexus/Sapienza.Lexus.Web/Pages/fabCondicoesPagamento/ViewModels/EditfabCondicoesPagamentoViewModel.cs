using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabCondicoesPagamento.ViewModels;

public class EditfabCondicoesPagamentoViewModel
{
    [Display(Name = "fabCondicoesPagamento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [Display(Name = "fabCondicoesPagamento:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabCondicoesPagamento:parcelas")]
    public int? parcelas { get; set; }
    [Display(Name = "fabCondicoesPagamento:p1")]
    public double? p1 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d1")]
    public int? d1 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p2")]
    public double? p2 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d2")]
    public int? d2 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p3")]
    public double? p3 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d3")]
    public int? d3 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p4")]
    public double? p4 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d4")]
    public int? d4 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p5")]
    public double? p5 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d5")]
    public int? d5 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p6")]
    public double? p6 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d6")]
    public int? d6 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p7")]
    public double? p7 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d7")]
    public int? d7 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p8")]
    public double? p8 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d8")]
    public int? d8 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p9")]
    public double? p9 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d9")]
    public int? d9 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p10")]
    public double? p10 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d10")]
    public int? d10 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p11")]
    public double? p11 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d11")]
    public int? d11 { get; set; }
    [Display(Name = "fabCondicoesPagamento:p12")]
    public double? p12 { get; set; }
    [Display(Name = "fabCondicoesPagamento:d12")]
    public int? d12 { get; set; }
    [Display(Name = "fabCondicoesPagamento:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabCondicoesPagamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabCondicoesPagamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fabCondicoesPagamento:compras")]
    public bool? compras { get; set; }
    [Display(Name = "fabCondicoesPagamento:vendas")]
    public bool? vendas { get; set; }
    [Display(Name = "fabCondicoesPagamento:valorMinimo")]
    public double? valorMinimo { get; set; }
    [Display(Name = "fabCondicoesPagamento:atendimento")]
    public bool? atendimento { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
