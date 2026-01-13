using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProOrgaos.ViewModels;

public class EditadvProOrgaosViewModel
{
    [Display(Name = "advProOrgaos:idOrgao")]
    public int? idOrgao { get; set; }
    [Display(Name = "advProOrgaos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advProOrgaos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProOrgaos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProOrgaos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
