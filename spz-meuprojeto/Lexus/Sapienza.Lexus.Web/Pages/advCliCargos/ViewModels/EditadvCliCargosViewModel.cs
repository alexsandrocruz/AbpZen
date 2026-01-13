using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliCargos.ViewModels;

public class EditadvCliCargosViewModel
{
    [Display(Name = "advCliCargos:idCargo")]
    public int? idCargo { get; set; }
    [Display(Name = "advCliCargos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliCargos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliCargos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliCargos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
