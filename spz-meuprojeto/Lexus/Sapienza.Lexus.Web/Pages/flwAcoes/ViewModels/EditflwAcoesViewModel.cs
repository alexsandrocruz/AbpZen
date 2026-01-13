using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.flwAcoes.ViewModels;

public class EditflwAcoesViewModel
{
    [Display(Name = "flwAcoes:idAcao")]
    public int? idAcao { get; set; }
    [Display(Name = "flwAcoes:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "flwAcoes:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "flwAcoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "flwAcoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "flwAcoes:diasReagendamento")]
    public int? diasReagendamento { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
