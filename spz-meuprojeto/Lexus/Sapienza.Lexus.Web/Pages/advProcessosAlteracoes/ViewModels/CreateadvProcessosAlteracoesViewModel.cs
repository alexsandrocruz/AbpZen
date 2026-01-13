using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advProcessosAlteracoes.ViewModels;

public class CreateadvProcessosAlteracoesViewModel
{
    [Display(Name = "advProcessosAlteracoes:idProcessoAlteracao")]
    public int? idProcessoAlteracao { get; set; }
    [Required]
    [Display(Name = "advProcessosAlteracoes:idProcesso")]
    public int idProcesso { get; set; }
    [Required]
    [Display(Name = "advProcessosAlteracoes:idUsuario")]
    public int idUsuario { get; set; }
    [Display(Name = "advProcessosAlteracoes:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "advProcessosAlteracoes:texto")]
    public string? texto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
