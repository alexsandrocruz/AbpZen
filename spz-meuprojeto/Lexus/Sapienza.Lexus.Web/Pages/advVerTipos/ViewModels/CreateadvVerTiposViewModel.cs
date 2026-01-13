using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advVerTipos.ViewModels;

public class CreateadvVerTiposViewModel
{
    [Display(Name = "advVerTipos:idTipo")]
    public int? idTipo { get; set; }
    [Display(Name = "advVerTipos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advVerTipos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advVerTipos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advVerTipos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
