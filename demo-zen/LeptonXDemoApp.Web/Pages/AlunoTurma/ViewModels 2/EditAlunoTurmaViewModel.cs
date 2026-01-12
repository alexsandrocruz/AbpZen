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

    // ========== Child Collections (1:N Master-Detail) ==========
}
