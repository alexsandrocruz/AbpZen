using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPostosINSS.ViewModels;

public class EditadvPostosINSSViewModel
{
    [Display(Name = "advPostosINSS:idPosto")]
    public int? idPosto { get; set; }
    [Display(Name = "advPostosINSS:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advPostosINSS:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advPostosINSS:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPostosINSS:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
