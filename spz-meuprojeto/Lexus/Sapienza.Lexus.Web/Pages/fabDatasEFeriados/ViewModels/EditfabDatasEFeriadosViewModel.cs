using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabDatasEFeriados.ViewModels;

public class EditfabDatasEFeriadosViewModel
{
    [Display(Name = "fabDatasEFeriados:idData")]
    public int? idData { get; set; }
    [Display(Name = "fabDatasEFeriados:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabDatasEFeriados:data")]
    public string? data { get; set; }
    [Display(Name = "fabDatasEFeriados:feriado")]
    public bool? feriado { get; set; }
    [Display(Name = "fabDatasEFeriados:fixo")]
    public bool? fixo { get; set; }
    [Display(Name = "fabDatasEFeriados:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabDatasEFeriados:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabDatasEFeriados:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
