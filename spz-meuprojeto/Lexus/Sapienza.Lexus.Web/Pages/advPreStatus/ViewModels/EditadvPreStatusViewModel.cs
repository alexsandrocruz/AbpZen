using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreStatus.ViewModels;

public class EditadvPreStatusViewModel
{
    [Display(Name = "advPreStatus:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advPreStatus:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advPreStatus:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advPreStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPreStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advPreStatus:ordem")]
    public int? ordem { get; set; }
    [Display(Name = "advPreStatus:ultimo")]
    public bool? ultimo { get; set; }
    [Display(Name = "advPreStatus:diasMaxParado")]
    public int? diasMaxParado { get; set; }
    [Display(Name = "advPreStatus:idTipo")]
    public int? idTipo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
