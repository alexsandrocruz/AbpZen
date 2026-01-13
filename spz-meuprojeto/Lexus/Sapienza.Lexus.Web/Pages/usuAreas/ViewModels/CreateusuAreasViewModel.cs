using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.usuAreas.ViewModels;

public class CreateusuAreasViewModel
{
    [Display(Name = "usuAreas:idArea")]
    public int? idArea { get; set; }
    [Display(Name = "usuAreas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "usuAreas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "usuAreas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "usuAreas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
