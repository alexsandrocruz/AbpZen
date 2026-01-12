using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.AlunoTurma.ViewModels;

public class EditAlunoTurmaViewModel
{
    [Required]
    [Display(Name = "AlunoTurma:DataMatricula")]
    public DateTime DataMatricula { get; set; }
    [StringLength(50)]
    [Display(Name = "AlunoTurma:Situacao")]
    public string? Situacao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    [Display(Name = "AlunoTurma:AlunoId")]
    [SelectItems(nameof(AlunoList))]
    public Guid AlunoId { get; set; }

    public List<SelectListItem> AlunoList { get; set; } = new();
    [Required]
    [Display(Name = "AlunoTurma:TurmaId")]
    [SelectItems(nameof(TurmaList))]
    public Guid TurmaId { get; set; }

    public List<SelectListItem> TurmaList { get; set; } = new();

    // ========== Child Collections (1:N Master-Detail) ==========
}
