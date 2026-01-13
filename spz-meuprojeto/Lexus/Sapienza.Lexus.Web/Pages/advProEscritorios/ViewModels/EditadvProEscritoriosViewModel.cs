using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProEscritorios.ViewModels;

public class EditadvProEscritoriosViewModel
{
    [Display(Name = "advProEscritorios:idEscritorio")]
    public int? idEscritorio { get; set; }
    [Display(Name = "advProEscritorios:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProEscritorios:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProEscritorios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProEscritorios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advProEscritorios:idCentroCusto")]
    public int? idCentroCusto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
