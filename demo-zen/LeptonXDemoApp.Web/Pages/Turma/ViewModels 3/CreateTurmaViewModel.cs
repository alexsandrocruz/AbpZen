using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Web.Pages.Turma.ViewModels;

public class CreateTurmaViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Turma:Nome")]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [StringLength(20)]
    [Display(Name = "Turma:Codigo")]
    public string Codigo { get; set; } = string.Empty;
    [Required]
    [Display(Name = "Turma:AnoLetivo")]
    public int AnoLetivo { get; set; }
    [Required]
    [Display(Name = "Turma:Semestre")]
    public int Semestre { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateAlunoTurmaDto> AlunoTurmas { get; set; } = new();
}
