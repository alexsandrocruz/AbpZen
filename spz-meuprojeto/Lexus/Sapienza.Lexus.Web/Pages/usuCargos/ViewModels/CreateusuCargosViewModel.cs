using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuCargos.ViewModels;

public class CreateusuCargosViewModel
{
    [Display(Name = "usuCargos:idCargo")]
    public int? idCargo { get; set; }
    [Required]
    [Display(Name = "usuCargos:idArea")]
    public int idArea { get; set; }
    [Display(Name = "usuCargos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "usuCargos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "usuCargos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "usuCargos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
