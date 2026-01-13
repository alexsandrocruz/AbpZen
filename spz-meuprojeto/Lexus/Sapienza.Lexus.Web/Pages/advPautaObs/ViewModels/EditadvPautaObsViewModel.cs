using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advPautaObs.ViewModels;

public class EditadvPautaObsViewModel
{
    [Display(Name = "advPautaObs:idPautaObs")]
    public int? idPautaObs { get; set; }
    [Display(Name = "advPautaObs:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advPautaObs:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advPautaObs:id")]
    public int? id { get; set; }
    [Display(Name = "advPautaObs:idTipo")]
    public string? idTipo { get; set; }
    [Display(Name = "advPautaObs:observacao")]
    public string? observacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
