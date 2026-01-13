using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabRegioes.ViewModels;

public class EditfabRegioesViewModel
{
    [Display(Name = "fabRegioes:idRegiao")]
    public int? idRegiao { get; set; }
    [Display(Name = "fabRegioes:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabRegioes:estados")]
    public string? estados { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
