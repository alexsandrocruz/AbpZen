using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabFormasPagamento.ViewModels;

public class EditfabFormasPagamentoViewModel
{
    [Display(Name = "fabFormasPagamento:idFormaPagamento")]
    public int? idFormaPagamento { get; set; }
    [Display(Name = "fabFormasPagamento:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabFormasPagamento:ordem")]
    public int? ordem { get; set; }
    [Display(Name = "fabFormasPagamento:padrao")]
    public bool? padrao { get; set; }
    [Display(Name = "fabFormasPagamento:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabFormasPagamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabFormasPagamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fabFormasPagamento:idCondicaoPagamento")]
    public int? idCondicaoPagamento { get; set; }
    [Display(Name = "fabFormasPagamento:contasPagar")]
    public bool? contasPagar { get; set; }
    [Display(Name = "fabFormasPagamento:compras")]
    public bool? compras { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
