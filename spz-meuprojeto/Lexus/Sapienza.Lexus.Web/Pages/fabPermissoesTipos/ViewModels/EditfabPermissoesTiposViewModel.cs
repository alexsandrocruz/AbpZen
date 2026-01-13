using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabPermissoesTipos.ViewModels;

public class EditfabPermissoesTiposViewModel
{
    [Display(Name = "fabPermissoesTipos:idPermissaoTipo")]
    public int? idPermissaoTipo { get; set; }
    [Display(Name = "fabPermissoesTipos:descricao")]
    public string? descricao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
