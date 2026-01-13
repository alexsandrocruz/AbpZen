using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProMeritos.ViewModels;

public class CreateadvProMeritosViewModel
{
    [Display(Name = "advProMeritos:idMerito")]
    public int? idMerito { get; set; }
    [Display(Name = "advProMeritos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProMeritos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProMeritos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProMeritos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advProMeritos:beneficioINSS")]
    public bool? beneficioINSS { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
