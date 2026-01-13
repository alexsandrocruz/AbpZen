using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesModelos.ViewModels;

public class EditadvClientesModelosViewModel
{
    [Display(Name = "advClientesModelos:idModelo")]
    public int? idModelo { get; set; }
    [Display(Name = "advClientesModelos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advClientesModelos:conteudo")]
    [TextArea(Rows = 3)]
    public string? conteudo { get; set; }
    [Display(Name = "advClientesModelos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advClientesModelos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientesModelos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
