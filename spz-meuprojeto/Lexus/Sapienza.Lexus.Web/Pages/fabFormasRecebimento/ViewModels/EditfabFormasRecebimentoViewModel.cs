using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabFormasRecebimento.ViewModels;

public class EditfabFormasRecebimentoViewModel
{
    [Display(Name = "fabFormasRecebimento:idFormaRecebimento")]
    public int? idFormaRecebimento { get; set; }
    [Display(Name = "fabFormasRecebimento:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabFormasRecebimento:ordem")]
    public int? ordem { get; set; }
    [Display(Name = "fabFormasRecebimento:padrao")]
    public bool? padrao { get; set; }
    [Display(Name = "fabFormasRecebimento:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabFormasRecebimento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabFormasRecebimento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fabFormasRecebimento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [Display(Name = "fabFormasRecebimento:online")]
    public bool? online { get; set; }
    [Display(Name = "fabFormasRecebimento:tipo")]
    public string? tipo { get; set; }
    [Display(Name = "fabFormasRecebimento:emailPagSeguro")]
    public string? emailPagSeguro { get; set; }
    [Display(Name = "fabFormasRecebimento:texto")]
    [TextArea(Rows = 3)]
    public string? texto { get; set; }
    [Display(Name = "fabFormasRecebimento:contasReceber")]
    public bool? contasReceber { get; set; }
    [Display(Name = "fabFormasRecebimento:vendas")]
    public bool? vendas { get; set; }
    [Display(Name = "fabFormasRecebimento:diasParaPrevisao")]
    public int? diasParaPrevisao { get; set; }
    [Display(Name = "fabFormasRecebimento:valorDesconto")]
    public double? valorDesconto { get; set; }
    [Display(Name = "fabFormasRecebimento:descontoTipo")]
    public string? descontoTipo { get; set; }
    [Display(Name = "fabFormasRecebimento:recebimentoFuturo")]
    public bool? recebimentoFuturo { get; set; }
    [Display(Name = "fabFormasRecebimento:recebimentoFuturoDias")]
    public int? recebimentoFuturoDias { get; set; }
    [Display(Name = "fabFormasRecebimento:recebimentoFuturoTaxa")]
    public double? recebimentoFuturoTaxa { get; set; }
    [Display(Name = "fabFormasRecebimento:idConta")]
    public int? idConta { get; set; }
    [Display(Name = "fabFormasRecebimento:idPlanoConta")]
    public int? idPlanoConta { get; set; }
    [Display(Name = "fabFormasRecebimento:idCentroCusto")]
    public int? idCentroCusto { get; set; }
    [Display(Name = "fabFormasRecebimento:idContaPagar")]
    public int? idContaPagar { get; set; }
    [Display(Name = "fabFormasRecebimento:idPlanoContaPagar")]
    public int? idPlanoContaPagar { get; set; }
    [Display(Name = "fabFormasRecebimento:idCentroCustoPagar")]
    public int? idCentroCustoPagar { get; set; }
    [Display(Name = "fabFormasRecebimento:idFormaPagar")]
    public int? idFormaPagar { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
