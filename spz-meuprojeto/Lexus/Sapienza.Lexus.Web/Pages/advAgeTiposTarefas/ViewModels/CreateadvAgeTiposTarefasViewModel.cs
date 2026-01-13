using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposTarefas.ViewModels;

public class CreateadvAgeTiposTarefasViewModel
{
    [Display(Name = "advAgeTiposTarefas:idTipoTarefa")]
    public int? idTipoTarefa { get; set; }
    [Display(Name = "advAgeTiposTarefas:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advAgeTiposTarefas:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advAgeTiposTarefas:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advAgeTiposTarefas:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advAgeTiposTarefas:agendada")]
    public bool? agendada { get; set; }
    [Display(Name = "advAgeTiposTarefas:pauta")]
    public bool? pauta { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
