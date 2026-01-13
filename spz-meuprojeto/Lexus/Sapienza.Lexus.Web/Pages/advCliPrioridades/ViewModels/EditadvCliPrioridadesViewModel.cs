using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliPrioridades.ViewModels;

public class EditadvCliPrioridadesViewModel
{
    [Display(Name = "advCliPrioridades:idPrioridade")]
    public int? idPrioridade { get; set; }
    [Display(Name = "advCliPrioridades:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliPrioridades:cor")]
    public string? cor { get; set; }
    [Display(Name = "advCliPrioridades:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliPrioridades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliPrioridades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
