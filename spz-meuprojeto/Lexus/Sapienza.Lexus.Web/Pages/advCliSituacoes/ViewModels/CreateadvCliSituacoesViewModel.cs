using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advCliSituacoes.ViewModels;

public class CreateadvCliSituacoesViewModel
{
    [Display(Name = "advCliSituacoes:idSituacao")]
    public int? idSituacao { get; set; }
    [Display(Name = "advCliSituacoes:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "advCliSituacoes:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "advCliSituacoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advCliSituacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
