using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliTiposHistoricos.ViewModels;

public class CreateadvCliTiposHistoricosViewModel
{
    [Display(Name = "advCliTiposHistoricos:idTipoHistorico")]
    public int? idTipoHistorico { get; set; }
    [Display(Name = "advCliTiposHistoricos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliTiposHistoricos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliTiposHistoricos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliTiposHistoricos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
