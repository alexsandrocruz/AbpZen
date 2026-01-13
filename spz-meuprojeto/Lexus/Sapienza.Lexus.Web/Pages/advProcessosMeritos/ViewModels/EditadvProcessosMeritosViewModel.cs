using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessosMeritos.ViewModels;

public class EditadvProcessosMeritosViewModel
{
    [Display(Name = "advProcessosMeritos:idProcessoMerito")]
    public int? idProcessoMerito { get; set; }
    [Required]
    [Display(Name = "advProcessosMeritos:idProcesso")]
    public int idProcesso { get; set; }
    [Required]
    [Display(Name = "advProcessosMeritos:idMerito")]
    public int idMerito { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
