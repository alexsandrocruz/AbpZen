using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliBairros.ViewModels;

public class EditadvCliBairrosViewModel
{
    [Display(Name = "advCliBairros:idBairro")]
    public int? idBairro { get; set; }
    [Display(Name = "advCliBairros:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliBairros:cidade")]
    public string? cidade { get; set; }
    [Display(Name = "advCliBairros:estado")]
    public string? estado { get; set; }
    [Display(Name = "advCliBairros:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliBairros:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliBairros:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
