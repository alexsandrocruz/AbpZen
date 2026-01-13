using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProProbabilidades.ViewModels;

public class CreateadvProProbabilidadesViewModel
{
    [Display(Name = "advProProbabilidades:idProbabilidade")]
    public int? idProbabilidade { get; set; }
    [Display(Name = "advProProbabilidades:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProProbabilidades:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProProbabilidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProProbabilidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
