using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProStatus.ViewModels;

public class CreateadvProStatusViewModel
{
    [Display(Name = "advProStatus:idStatus")]
    public int? idStatus { get; set; }
    [Display(Name = "advProStatus:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProStatus:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProStatus:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProStatus:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
