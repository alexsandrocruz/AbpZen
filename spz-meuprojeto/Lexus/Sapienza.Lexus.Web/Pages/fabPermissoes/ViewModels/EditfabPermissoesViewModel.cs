using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabPermissoes.ViewModels;

public class EditfabPermissoesViewModel
{
    [Display(Name = "fabPermissoes:idPermissao")]
    public int? idPermissao { get; set; }
    [Required]
    [Display(Name = "fabPermissoes:idPermissaoTipo")]
    public int idPermissaoTipo { get; set; }
    [Display(Name = "fabPermissoes:modulo")]
    public bool? modulo { get; set; }
    [Display(Name = "fabPermissoes:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "fabPermissoes:varSession")]
    public string? varSession { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
