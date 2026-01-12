using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.AlunoTurma.Dtos;

namespace LeptonXDemoApp.Web.Pages.Aluno.ViewModels;

public class CreateAlunoViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Aluno:Nome")]
    public string Nome { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    [Display(Name = "Aluno:Email")]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(20)]
    [Display(Name = "Aluno:Matricula")]
    public string Matricula { get; set; } = string.Empty;
    [Display(Name = "Aluno:DataNascimento")]
    public DateTime? DataNascimento { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateAlunoTurmaDto> AlunoTurmas { get; set; } = new();
}
