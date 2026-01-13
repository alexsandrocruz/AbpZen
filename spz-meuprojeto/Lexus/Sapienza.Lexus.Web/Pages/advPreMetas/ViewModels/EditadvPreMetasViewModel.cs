using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPreMetas.ViewModels;

public class EditadvPreMetasViewModel
{
    [Display(Name = "advPreMetas:idMeta")]
    public int? idMeta { get; set; }
    [Display(Name = "advPreMetas:tipo")]
    public string? tipo { get; set; }
    [Display(Name = "advPreMetas:idResponsavel")]
    public int? idResponsavel { get; set; }
    [Display(Name = "advPreMetas:idEscritorio")]
    public int? idEscritorio { get; set; }
    [Display(Name = "advPreMetas:qtde")]
    public int? qtde { get; set; }
    [Display(Name = "advPreMetas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
