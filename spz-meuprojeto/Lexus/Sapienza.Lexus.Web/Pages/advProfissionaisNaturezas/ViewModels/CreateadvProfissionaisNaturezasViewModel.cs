using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas.ViewModels;

public class CreateadvProfissionaisNaturezasViewModel
{
    [Display(Name = "advProfissionaisNaturezas:idProfissionalNatureza")]
    public int? idProfissionalNatureza { get; set; }
    [Required]
    [Display(Name = "advProfissionaisNaturezas:idProfissional")]
    public int idProfissional { get; set; }
    [Required]
    [Display(Name = "advProfissionaisNaturezas:idNatureza")]
    public int idNatureza { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
