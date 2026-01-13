using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finExtrato.ViewModels;

public class CreatefinExtratoViewModel
{
    [Display(Name = "finExtrato:idExtrato")]
    public int? idExtrato { get; set; }
    [Required]
    [Display(Name = "finExtrato:idConta")]
    public int idConta { get; set; }
    [Display(Name = "finExtrato:idLancamento")]
    public int? idLancamento { get; set; }
    [Display(Name = "finExtrato:transferencia")]
    public bool? transferencia { get; set; }
    [Display(Name = "finExtrato:idExtratoRel")]
    public int? idExtratoRel { get; set; }
    [Display(Name = "finExtrato:data")]
    public string? data { get; set; }
    [Display(Name = "finExtrato:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "finExtrato:credito")]
    public double? credito { get; set; }
    [Display(Name = "finExtrato:debito")]
    public double? debito { get; set; }
    [Display(Name = "finExtrato:conferido")]
    public bool? conferido { get; set; }
    [Display(Name = "finExtrato:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finExtrato:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finExtrato:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finExtrato:idUsuarioInclusao")]
    public int? idUsuarioInclusao { get; set; }
    [Display(Name = "finExtrato:idUsuarioAlteracao")]
    public int? idUsuarioAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
