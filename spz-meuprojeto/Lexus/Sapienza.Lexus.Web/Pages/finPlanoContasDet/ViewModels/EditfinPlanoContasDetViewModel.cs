using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasDet.ViewModels;

public class EditfinPlanoContasDetViewModel
{
    [Display(Name = "finPlanoContasDet:idPlanoContasDet")]
    public int? idPlanoContasDet { get; set; }
    [Required]
    [Display(Name = "finPlanoContasDet:idPlanoConta")]
    public int idPlanoConta { get; set; }
    [Display(Name = "finPlanoContasDet:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "finPlanoContasDet:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "finPlanoContasDet:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "finPlanoContasDet:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
