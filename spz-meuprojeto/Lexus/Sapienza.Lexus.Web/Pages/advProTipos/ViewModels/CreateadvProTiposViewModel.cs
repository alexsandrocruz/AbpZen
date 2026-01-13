using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProTipos.ViewModels;

public class CreateadvProTiposViewModel
{
    [Display(Name = "advProTipos:idTipo")]
    public int? idTipo { get; set; }
    [Display(Name = "advProTipos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProTipos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
