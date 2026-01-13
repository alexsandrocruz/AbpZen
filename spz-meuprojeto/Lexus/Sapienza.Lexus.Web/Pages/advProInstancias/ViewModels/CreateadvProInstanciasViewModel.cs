using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProInstancias.ViewModels;

public class CreateadvProInstanciasViewModel
{
    [Display(Name = "advProInstancias:idInstancia")]
    public int? idInstancia { get; set; }
    [Display(Name = "advProInstancias:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProInstancias:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProInstancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProInstancias:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
