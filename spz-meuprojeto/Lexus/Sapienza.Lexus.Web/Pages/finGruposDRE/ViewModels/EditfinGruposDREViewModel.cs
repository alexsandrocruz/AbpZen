using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finGruposDRE.ViewModels;

public class EditfinGruposDREViewModel
{
    [Display(Name = "finGruposDRE:idGrupoDRE")]
    public int? idGrupoDRE { get; set; }
    [Display(Name = "finGruposDRE:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finGruposDRE:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finGruposDRE:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finGruposDRE:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
