using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesINSSStatus.ViewModels;

public class CreateadvClientesINSSStatusViewModel
{
    [Display(Name = "advClientesINSSStatus:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advClientesINSSStatus:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advClientesINSSStatus:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advClientesINSSStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advClientesINSSStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
