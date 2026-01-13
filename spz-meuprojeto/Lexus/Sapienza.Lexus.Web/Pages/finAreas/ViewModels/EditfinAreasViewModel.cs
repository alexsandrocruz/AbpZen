using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finAreas.ViewModels;

public class EditfinAreasViewModel
{
    [Display(Name = "finAreas:idArea")]
    public int? idArea { get; set; }
    [Display(Name = "finAreas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finAreas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finAreas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finAreas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finAreas:idCentroResultado")]
    public int? idCentroResultado { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
