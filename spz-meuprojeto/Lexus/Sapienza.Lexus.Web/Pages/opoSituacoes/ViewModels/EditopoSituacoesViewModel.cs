using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.opoSituacoes.ViewModels;

public class EditopoSituacoesViewModel
{
    [Display(Name = "opoSituacoes:idSituacao")]
    public int? idSituacao { get; set; }
    [Display(Name = "opoSituacoes:titulo")]
    public string? titulo { get; set; }
    [Display(Name = "opoSituacoes:ativo")]
    public bool? ativo { get; set; }
    [Display(Name = "opoSituacoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "opoSituacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "opoSituacoes:ordem")]
    public int? ordem { get; set; }
    [Display(Name = "opoSituacoes:considerarIndicador")]
    public bool? considerarIndicador { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
