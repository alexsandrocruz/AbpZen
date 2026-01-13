using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.flwConfig.ViewModels;

public class EditflwConfigViewModel
{
    [Display(Name = "flwConfig:idConfig")]
    public int? idConfig { get; set; }
    [Display(Name = "flwConfig:tipoMarcacoes")]
    public string? tipoMarcacoes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
