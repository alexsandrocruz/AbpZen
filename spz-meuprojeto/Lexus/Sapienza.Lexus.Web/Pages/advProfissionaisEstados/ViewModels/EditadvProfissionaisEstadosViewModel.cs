using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisEstados.ViewModels;

public class EditadvProfissionaisEstadosViewModel
{
    [Display(Name = "advProfissionaisEstados:idProfissionalEstado")]
    public int? idProfissionalEstado { get; set; }
    [Required]
    [Display(Name = "advProfissionaisEstados:idProfissional")]
    public int idProfissional { get; set; }
    [Display(Name = "advProfissionaisEstados:estado")]
    public string? estado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
