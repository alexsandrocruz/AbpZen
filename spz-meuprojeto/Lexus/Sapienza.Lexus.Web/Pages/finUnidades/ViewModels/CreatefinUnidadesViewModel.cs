using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finUnidades.ViewModels;

public class CreatefinUnidadesViewModel
{
    [Display(Name = "finUnidades:idUnidade")]
    public int? idUnidade { get; set; }
    [Display(Name = "finUnidades:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finUnidades:percentual")]
    public double? percentual { get; set; }
    [Display(Name = "finUnidades:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finUnidades:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finUnidades:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
