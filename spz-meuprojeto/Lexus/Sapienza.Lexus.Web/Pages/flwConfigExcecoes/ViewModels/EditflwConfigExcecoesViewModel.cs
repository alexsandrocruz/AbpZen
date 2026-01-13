using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.flwConfigExcecoes.ViewModels;

public class EditflwConfigExcecoesViewModel
{
    [Display(Name = "flwConfigExcecoes:idConfig")]
    public int? idConfig { get; set; }
    [Display(Name = "flwConfigExcecoes:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }
    [Required]
    [Display(Name = "flwConfigExcecoes:idHistoricoTipo")]
    public int idHistoricoTipo { get; set; }
    [Display(Name = "flwConfigExcecoes:data")]
    public string? data { get; set; }
    [Display(Name = "flwConfigExcecoes:qtde")]
    public int? qtde { get; set; }
    [Display(Name = "flwConfigExcecoes:manhaQtde")]
    public int? manhaQtde { get; set; }
    [Display(Name = "flwConfigExcecoes:tardeQtde")]
    public int? tardeQtde { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
