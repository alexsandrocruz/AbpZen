using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.fdtDevs.ViewModels;

public class CreatefdtDevsViewModel
{
    [Display(Name = "fdtDevs:idDev")]
    public int? idDev { get; set; }
    [Display(Name = "fdtDevs:pacote")]
    public string? pacote { get; set; }
    [Display(Name = "fdtDevs:descricao")]
    public string? descricao { get; set; }
    [Display(Name = "fdtDevs:pendente")]
    public bool? pendente { get; set; }
    [Display(Name = "fdtDevs:aprovado")]
    public bool? aprovado { get; set; }
    [Display(Name = "fdtDevs:reprovado")]
    public bool? reprovado { get; set; }
    [Display(Name = "fdtDevs:finalizado")]
    public bool? finalizado { get; set; }
    [Display(Name = "fdtDevs:comentariosRevisor")]
    public string? comentariosRevisor { get; set; }
    [Display(Name = "fdtDevs:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [Display(Name = "fdtDevs:incluidoPor")]
    public string? incluidoPor { get; set; }
    [Display(Name = "fdtDevs:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "fdtDevs:alteradoPor")]
    public string? alteradoPor { get; set; }
    [Display(Name = "fdtDevs:ativo")]
    public bool? ativo { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
