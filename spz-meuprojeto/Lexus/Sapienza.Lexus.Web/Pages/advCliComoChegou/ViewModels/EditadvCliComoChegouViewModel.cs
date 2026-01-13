using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliComoChegou.ViewModels;

public class EditadvCliComoChegouViewModel
{
    [Display(Name = "advCliComoChegou:idComoChegou")]
    public int? idComoChegou { get; set; }
    [Display(Name = "advCliComoChegou:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliComoChegou:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliComoChegou:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliComoChegou:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
