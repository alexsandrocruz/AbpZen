using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasGrupos.ViewModels;

public class CreatefinPlanoContasGruposViewModel
{
    [Display(Name = "finPlanoContasGrupos:idGrupo")]
    public int? idGrupo { get; set; }
    [Display(Name = "finPlanoContasGrupos:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finPlanoContasGrupos:tipo")]
    public string? tipo { get; set; }
    [Display(Name = "finPlanoContasGrupos:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finPlanoContasGrupos:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finPlanoContasGrupos:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "finPlanoContasGrupos:idGrupoDRE")]
    public int? idGrupoDRE { get; set; }
    [Display(Name = "finPlanoContasGrupos:ordem")]
    public int? ordem { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
