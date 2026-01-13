using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fabMotivosAproveitamento.ViewModels;

public class CreatefabMotivosAproveitamentoViewModel
{
    [Display(Name = "fabMotivosAproveitamento:idMotivo")]
    public int? idMotivo { get; set; }
    [Display(Name = "fabMotivosAproveitamento:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "fabMotivosAproveitamento:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "fabMotivosAproveitamento:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fabMotivosAproveitamento:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
