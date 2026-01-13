using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProfissionais.ViewModels;

public class CreateadvProfissionaisViewModel
{
    [Display(Name = "advProfissionais:idProfissional")]
    public int? idProfissional { get; set; }
    [Required]
    [Display(Name = "advProfissionais:idUsuario")]
    public int idUsuario { get; set; }
    [Display(Name = "advProfissionais:nome")]
    public string? nome { get; set; }
    [Display(Name = "advProfissionais:email")]
    public string? email { get; set; }
    [Display(Name = "advProfissionais:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advProfissionais:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProfissionais:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
