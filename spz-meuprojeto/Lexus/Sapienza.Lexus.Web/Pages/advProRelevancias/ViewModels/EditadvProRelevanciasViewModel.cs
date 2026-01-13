using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProRelevancias.ViewModels;

public class EditadvProRelevanciasViewModel
{
    [Display(Name = "advProRelevancias:idRelevancia")]
    public int? idRelevancia { get; set; }
    [Display(Name = "advProRelevancias:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProRelevancias:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProRelevancias:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProRelevancias:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
