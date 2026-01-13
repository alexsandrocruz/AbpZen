using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliLocaisAtendido.ViewModels;

public class EditadvCliLocaisAtendidoViewModel
{
    [Display(Name = "advCliLocaisAtendido:idLocalAtendido")]
    public int? idLocalAtendido { get; set; }
    [Display(Name = "advCliLocaisAtendido:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliLocaisAtendido:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliLocaisAtendido:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliLocaisAtendido:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
