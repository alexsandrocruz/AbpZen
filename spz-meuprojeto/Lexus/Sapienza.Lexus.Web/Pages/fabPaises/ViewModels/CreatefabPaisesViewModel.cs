using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabPaises.ViewModels;

public class CreatefabPaisesViewModel
{
    [Display(Name = "fabPaises:idPais")]
    public int? idPais { get; set; }
    [Display(Name = "fabPaises:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabPaises:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabPaises:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabPaises:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
